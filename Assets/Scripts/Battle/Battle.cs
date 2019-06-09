using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    public DiceSet playerDices; // array of player dices on board
    public DiceSet enemyDices; // array of enemy dices on board
    public HealthBar playerHB; // player health bar
    public Text playerHPtext; // player health points text
    public HealthBar enemyHB; // enemy health bar
    public Text enemyHPtext; // player health points text
    public Text rollButtonText; // link to roll button text (roll and reroll)
    public ItemList itemList; // link to item list

    private int playerMaxHP; // player max hp
    private int playerHP; // player HP
    private int enemyMaxHP; // enemy max hp
    private int enemyHP; // enemy HP
    private short stage; // stage of round
    private bool enemyRolled;

    // Start is called before the first frame update
    void Start()
    {
        // set variables
        if (!PlayerPrefs.HasKey("playerHP")) PlayerPrefs.SetInt("playerHP", 100);
        playerMaxHP = PlayerPrefs.GetInt("playerHP");
        playerHP = playerMaxHP;
        playerHPtext.text = playerHP.ToString();

        if (!PlayerPrefs.HasKey("npcHP")) PlayerPrefs.SetInt("npcHP", 100);
        enemyMaxHP = PlayerPrefs.GetInt("npcHP");
        enemyHP = enemyMaxHP;
        enemyHPtext.text = enemyHP.ToString();

        stage = 0;
        enemyRolled = false;
        rollButtonText.text = "ROLL";
    }

    // return stage phase
    public short GetStage() { return stage; }

    // check if battle should end
    private void EndCheck()
    {
        if (playerHP <= 0)
        {
            // TO IMPLEMENT - more stuff to do, some punishment for losing
            SceneManager.LoadScene(PlayerPrefs.GetString("lastScene"));
        }

        if (enemyHP <= 0)
        {
            PlayerPrefs.SetInt("playerEXP", PlayerPrefs.GetInt("playerEXP") + PlayerPrefs.GetInt("expGain"));
            Debug.Log("Player EXP: " + PlayerPrefs.GetInt("playerEXP"));
            SceneManager.LoadScene(PlayerPrefs.GetString("lastScene"));
        }
    }

    // roll button event
    public void OnRollButtonClick()
    {
        switch (stage) // check stage of round
        {
            case 0: // dices has not been rolled yet > reroll selected dices
                stage = 1;
                enemyRolled = false;
                enemyDices.RollSet();
                playerDices.RollSet();
                rollButtonText.text = "REROLL";
                break;

            case 1: // all dices were rolled once > lock roll button
                if (!enemyRolled)
                {
                    enemyDices.AIreroll(playerDices.GetOutcome());
                    enemyRolled = true;
                }

                stage = 2;
                playerDices.RerollSet();
                rollButtonText.text = "DECIDE";
                break;

            default:
                break;
        }
    }

    // defend button event
    public void OnDefendButtonClick()
    {
        if (!enemyRolled)
        {
            enemyDices.AIreroll(playerDices.GetOutcome());
            enemyRolled = true;
        }

        if (stage != 0) // only avaliable if dices were rolled at least once
        {
            if (enemyDices.GetOutcome() > playerDices.GetOutcome()) // if player hp is to be drained, else nothing happens
            {
                playerHP -= enemyDices.GetOutcome() - playerDices.GetOutcome();
                playerHB.RemoveHP(5.625f / (float)playerMaxHP * (float)enemyDices.GetOutcome());
                if (playerHP <= 0) playerHP = 0;
                playerHPtext.text = playerHP.ToString();
            }
            EndCheck();
            stage = 0;
            rollButtonText.text = "ROLL";
        }
    }

    // attack button event
    public void OnAttackButtonClick()
    {
        if (!enemyRolled)
        {
            enemyDices.AIreroll(playerDices.GetOutcome());
            enemyRolled = true;
        }

        if (stage != 0) // only avaliable if dices were rolled at least once
        {
            // enemy HP
            enemyHP -= playerDices.GetOutcome();
            enemyHB.RemoveHP(5.594886f / (float)enemyMaxHP * (float)playerDices.GetOutcome());
            if (enemyHP <= 0) enemyHP = 0;
            enemyHPtext.text = enemyHP.ToString();

            // player HP
            playerHP -= enemyDices.GetOutcome();
            playerHB.RemoveHP(5.594886f / (float)playerMaxHP * (float)enemyDices.GetOutcome());
            if (playerHP <= 0) playerHP = 0;
            playerHPtext.text = playerHP.ToString();

            EndCheck();
            stage = 0;
            rollButtonText.text = "ROLL";
        }
    }

    // when item is selected
    public void OnItemSelected(int id)
    {
        if (itemList.GetItemQuantity(id) > 0 && enemyHP + itemList.GetEnemyBuff(id) > 0 && playerHP + itemList.GetHeroBuff(id) <= playerMaxHP)
        {
            Camera.main.transform.position = new Vector3(0f, 0f, -1f);
            enemyHB.RemoveHP(5.594886f / (float)enemyMaxHP * -itemList.GetEnemyBuff(id));
            enemyHP += itemList.GetEnemyBuff(id);
            enemyHPtext.text = enemyHP.ToString();
            playerHB.AddHP(5.594886f / (float)playerMaxHP * itemList.GetHeroBuff(id));
            playerHP += itemList.GetHeroBuff(id);
            playerHPtext.text = playerHP.ToString();
            itemList.gameObject.SetActive(false);
            GameObject.FindObjectOfType<Equipment>().RemoveItem(itemList.GetItem(id));
            itemList.ChangeResetStatus();
        }
    }
}