using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Equipment equipment; // link to equipmnet
    public Button battleButton; // link to battle button for toggle
    public Button fleeButton; // link to flee button for toggle
    public float speed = 0.03f; // player speed
    private int playerHP = 100; // player health

    private int terceScreenWidth; // store terce of screen width
    private int terceScreenHeight; // store terce of screen width

    // Start is called before the first frame update
    void Start()
    {
        // get player hp from playerpref, if entry does not exist set default
        if (PlayerPrefs.HasKey("playerHP")) playerHP = PlayerPrefs.GetInt("playerHP");
        else
        {
            PlayerPrefs.SetInt("playerHP", playerHP);
        }

        // if playerpref has no entry for player experiance points create it with default value
        if (!PlayerPrefs.HasKey("playerEXP")) PlayerPrefs.SetInt("playerEXP", 0);

        terceScreenWidth = Screen.width / 3;
        terceScreenHeight = Screen.height / 3;

        // player position
        if (PlayerPrefs.HasKey("exitPositionX")) transform.position = new Vector3(PlayerPrefs.GetFloat("exitPositionX"), PlayerPrefs.GetFloat("exitPositionY"), 0f);
    }

    public int GetPlayerHP() { return playerHP; } // return player hp

    private void Update()
    {
        // check wich area was touched
        if (Input.GetKey(KeyCode.LeftArrow)) // 6, 7, 8
        {
            if (Input.GetKey(KeyCode.DownArrow)) // 6
            {
                transform.position = new Vector3(transform.position.x - speed, transform.position.y - speed, 0.0f); // move left-down
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 135.0f); // rotate to left-down
            }
            else if (Input.GetKey(KeyCode.UpArrow)) // 8
            {
                transform.position = new Vector3(transform.position.x - speed, transform.position.y + speed, 0.0f); // move left-up
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 45.0f);// rotate left-up
            }
            else // 7
            {
                transform.position = new Vector3(transform.position.x - speed, transform.position.y, 0.0f); // move left
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f); // rotate left
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // 2, 3, 4
        {
            if (Input.GetKey(KeyCode.DownArrow)) // 4
            {
                transform.position = new Vector3(transform.position.x + speed, transform.position.y - speed, 0.0f); // move right-down
                transform.eulerAngles = new Vector3(0.0f, 0.0f, -135.0f); // rotate right-down
            }
            else if (Input.GetKey(KeyCode.UpArrow)) // 2
            {
                transform.position = new Vector3(transform.position.x + speed, transform.position.y + speed, 0.0f); // move right-up
                transform.eulerAngles = new Vector3(0.0f, 0.0f, -45.0f); // rotate right-up
            }
            else // 3
            {
                transform.position = new Vector3(transform.position.x + speed, transform.position.y, 0.0f); // move right
                transform.eulerAngles = new Vector3(0.0f, 0.0f, -90.0f); // rotate right
            }
        }
        else // 1, 5, P
        {
            if (Input.GetKey(KeyCode.DownArrow)) // 5
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed, 0.0f); // move down
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f); // rotate down
            }
            else if (Input.GetKey(KeyCode.UpArrow)) // 1
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed, 0.0f); // move up
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f); // rotate up
            }
            else // Player
            {
                // display menu
            }
        }
    }

    // when trigger is entered
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("item")) // collided with item
        {
            equipment.AddItem(other.gameObject.GetComponent<Item>().ItemToStruct());
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("building")) // collided with building door
        {
            transform.position = new Vector3(other.gameObject.GetComponent<Building>().exitPositionX, other.gameObject.GetComponent<Building>().exitPositionY, 0f);
        }

        if (other.gameObject.CompareTag("enemy")) // collided with npc
        {
            speed = 0f;
            PlayerPrefs.SetInt("npcHP", other.gameObject.GetComponent<NPC>().npcHP);
            PlayerPrefs.SetInt("expGain", other.gameObject.GetComponent<NPC>().expGain);
            PlayerPrefs.SetFloat("exitPositionX", other.gameObject.GetComponent<NPC>().exitPositionX);
            PlayerPrefs.SetFloat("exitPositionY", other.gameObject.GetComponent<NPC>().exitPositionY);
            battleButton.gameObject.SetActive(true);
            fleeButton.gameObject.SetActive(true);
        }
    }

    public void OnBattleButtonClick()
    {
        PlayerPrefs.SetInt("playerHP", playerHP);
        PlayerPrefs.SetString("lastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("BattleScene");
    }

    public void OnFleeButtonClick()
    {
        battleButton.gameObject.SetActive(false);
        fleeButton.gameObject.SetActive(false);
        speed = 0.025f;
    }
}




// actual code for movement
/*
    if (Input.touchCount > 0) // if touch appears
    {
        Touch touch = Input.GetTouch(0); // bind first touch to object

        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        {
            // check wich area was touched
            if (touch.position.x < terceScreenWidth) // 6, 7, 8
            {
                if (touch.position.y < terceScreenHeight) // 6
                {
                    transform.position = new Vector3(transform.position.x - speed, transform.position.y - speed, 0.0f); // move left-down
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, 135.0f); // rotate to left-down
                }
                else if (touch.position.y > terceScreenHeight * 2) // 8
                {
                    transform.position = new Vector3(transform.position.x - speed, transform.position.y + speed, 0.0f); // move left-up
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, 45.0f);// rotate left-up
                }
                else // 7
                {
                    transform.position = new Vector3(transform.position.x - speed, transform.position.y, 0.0f); // move left
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f); // rotate left
                }
            }
            else if (touch.position.x > terceScreenWidth * 2) // 2, 3, 4
            {
                if (touch.position.y < terceScreenHeight) // 4
                {
                    transform.position = new Vector3(transform.position.x + speed, transform.position.y - speed, 0.0f); // move right-down
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, -135.0f); // rotate right-down
                }
                else if (touch.position.y > terceScreenHeight * 2) // 2
                {
                    transform.position = new Vector3(transform.position.x + speed, transform.position.y + speed, 0.0f); // move right-up
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, -45.0f); // rotate right-up
                }
                else // 3
                {
                    transform.position = new Vector3(transform.position.x + speed, transform.position.y, 0.0f); // move right
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, -90.0f); // rotate right
                }
            }
            else // 1, 5, P
            {
                if (touch.position.y < terceScreenHeight) // 5
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - speed, 0.0f); // move down
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f); // rotate down
                }
                else if (touch.position.y > terceScreenHeight * 2) // 1
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + speed, 0.0f); // move up
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f); // rotate up
                }
                else // Player
                {
                    // display menu
                }
            }
        }
    }
}*/