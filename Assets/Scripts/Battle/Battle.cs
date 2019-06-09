/*
 * Author: Emanuel Misztal
 * 2019
 */

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
        if (!PlayerPrefs.HasKey("playerHP")) PlayerPrefs.SetInt("playerHP", 100); // save default player hp if is't saved
        playerMaxHP = PlayerPrefs.GetInt("playerHP"); // read player max hp
        playerHP = playerMaxHP; // current player hp on begining is his max hp
        playerHPtext.text = playerHP.ToString(); // put hp to text field

        if (!PlayerPrefs.HasKey("npcHP")) PlayerPrefs.SetInt("npcHP", 100); // save default enemy hp if is't saved
        enemyMaxHP = PlayerPrefs.GetInt("npcHP"); // read enemy max hp
        enemyHP = enemyMaxHP; // current enemy hp on begining is his max hp
        enemyHPtext.text = enemyHP.ToString(); // put hp to text field

        stage = 0; // set stage phase to 0 (initial)
        enemyRolled = false; // enemy did not rolled dices yet
        rollButtonText.text = "ROLL"; // set text to ROLL on roll button
    }

    // return stage phase
    public short GetStage() { return stage; }

    // check if battle should end
    private void EndCheck()
    {
        if (playerHP <= 0) // if player hp is less or equals 0
        {
            // TO IMPLEMENT - more stuff to do, some punishment for losing
            //idea - exp is money
            SceneManager.LoadScene(PlayerPrefs.GetString("lastScene")); // load last scene
        }

        if (enemyHP <= 0) // if enemy hp is less or equals 0
        {
            PlayerPrefs.SetInt("playerEXP", PlayerPrefs.GetInt("playerEXP") + PlayerPrefs.GetInt("expGain")); // add to player exp
            SceneManager.LoadScene(PlayerPrefs.GetString("lastScene")); // load last scene
        }
    }

    // roll button event
    public void OnRollButtonClick()
    {
        switch (stage) // check stage of round
        {
            case 0: // dices has not been rolled yet > reroll selected dices
                stage = 1; // set stage phase to 1
                enemyRolled = false; // enemy did not rolled yet
                enemyDices.RollSet(); // roll enemy dices
                playerDices.RollSet(); // roll player dices
                rollButtonText.text = "REROLL"; // change roll text to reroll on roll button
                break;

            case 1: // all dices were rolled once > lock roll button
                if (!enemyRolled) // if enemy did not rolled yet
                {
                    enemyDices.AIreroll(playerDices.GetOutcome()); // reroll enemy dices
                    enemyRolled = true; // enemy rolled
                }

                stage = 2; // set stage phase to 2
                playerDices.RerollSet(); // reroll player dices (selected only)
                rollButtonText.text = "DECIDE"; // change roll button text to decide
                break;

            default:
                break;
        }
    }

    // defend button event
    public void OnDefendButtonClick()
    {
        if (!enemyRolled) // if enemy did not rolled yet
        {
            enemyDices.AIreroll(playerDices.GetOutcome()); // reroll enemy dices
            enemyRolled = true; // enemy rolled
        }

        if (stage != 0) // only avaliable if dices were rolled at least once
        {
            if (enemyDices.GetOutcome() > playerDices.GetOutcome()) // if player hp is to be drained, else nothing happens
            {
                playerHP -= enemyDices.GetOutcome() - playerDices.GetOutcome(); // decrement player hp
                playerHB.RemoveHP(5.625f / (float)playerMaxHP * (float)enemyDices.GetOutcome()); // move hp bar
                if (playerHP <= 0) playerHP = 0; // check if player hp isn't below 0
                playerHPtext.text = playerHP.ToString(); // change hp points text
            }

            EndCheck(); // check if battle should end
            stage = 0; // reset stage index
            rollButtonText.text = "ROLL"; // return roll button text to roll
        }
    }

    // attack button event
    public void OnAttackButtonClick()
    {
        if (!enemyRolled) // if enemy did not rolled yet
        {
            enemyDices.AIreroll(playerDices.GetOutcome()); // reroll enemy dices
            enemyRolled = true; // enemy rolled
        }

        if (stage != 0) // only avaliable if dices were rolled at least once
        {
            // enemy HP
            enemyHP -= playerDices.GetOutcome(); // decrement enemy hp
            enemyHB.RemoveHP(5.594886f / (float)enemyMaxHP * (float)playerDices.GetOutcome()); // move enemy hp bar
            if (enemyHP <= 0) enemyHP = 0; // check if enemy hp isn't below 0
            enemyHPtext.text = enemyHP.ToString(); // change hp points text

            // player HP
            playerHP -= enemyDices.GetOutcome(); // decrement hero hp
            playerHB.RemoveHP(5.594886f / (float)playerMaxHP * (float)enemyDices.GetOutcome()); // move hero hp bar
            if (playerHP <= 0) playerHP = 0; // check if hero hp isn't below 0
            playerHPtext.text = playerHP.ToString(); // change hp points text

            EndCheck(); // check if battle should end
            stage = 0; // reset stage index
            rollButtonText.text = "ROLL"; // return roll button text to roll 
        }
    }

    // when item is selected
    public void OnItemSelected(int id)
    {
        // if item quantity is above 0 and debuff won't degrade enemy hp below 0 and buff won't expand hero hp above max
        if (itemList.GetItemQuantity(id) > 0 && enemyHP + itemList.GetEnemyBuff(id) > 0 && playerHP + itemList.GetHeroBuff(id) <= playerMaxHP)
        {
            Camera.main.transform.position = new Vector3(0f, 0f, -1f); // move camera back to battle
            enemyHB.RemoveHP(5.594886f / (float)enemyMaxHP * -itemList.GetEnemyBuff(id)); // move enemy hp bar
            enemyHP += itemList.GetEnemyBuff(id); // change enemy hp
            enemyHPtext.text = enemyHP.ToString(); // change enemy hp text
            playerHB.AddHP(5.594886f / (float)playerMaxHP * itemList.GetHeroBuff(id)); // move player hp bar
            playerHP += itemList.GetHeroBuff(id); // change player hp
            playerHPtext.text = playerHP.ToString(); // change hero hp text
            itemList.gameObject.SetActive(false); // turn off item list
            GameObject.FindObjectOfType<Equipment>().RemoveItem(itemList.GetItem(id)); // remove item from inventory
            itemList.ChangeResetStatus(); // on next inventory visit list will be reloaded from eq
        }
    }
}