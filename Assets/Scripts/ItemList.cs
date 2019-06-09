using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public Button[] itemButtons; // link to each item button
    public GameObject[] itemSprites; // link to item list sprites
    public Text[] itemDesc; // link to item list descriptions

    private List<ItemStruct> itemList; // list of items
    private List<int> quantinty;
    private List<int> enemyBuff;
    private List<int> heroBuff;
    private bool toReset = true; // flag to mark item list to be reloaded

    public void OnItemsButtonClick()
    {
        gameObject.SetActive(true);
        Camera.main.transform.position = new Vector3(0f, 13f, -1f);

        if (toReset)
        {
            itemList = new List<ItemStruct>();
            enemyBuff = new List<int>();
            heroBuff = new List<int>();
            quantinty = new List<int>();
            int i = 0;

            foreach (KeyValuePair<ItemStruct, int> item in GameObject.FindObjectOfType<Equipment>().GetItemList())
            {
                itemList.Add(item.Key);
                quantinty.Add(item.Value);
                itemSprites[i].GetComponent<SpriteRenderer>().sprite = item.Key.sprite; // load item sprite
                itemDesc[i].text = item.Key.description + " X " + quantinty[i]; // load item description and quantity
                itemButtons[i].gameObject.SetActive(true); // activate button
                enemyBuff.Add(item.Key.enemyHP);
                heroBuff.Add(item.Key.heroHP);
                i++;
            }

            toReset = false;
        }
    }

    // exit items button event
    public void OnExitItemsButtonClick()
    {
        Camera.main.transform.position = new Vector3(0f, 0f, -1f);
        gameObject.SetActive(false);
    }

    public ItemStruct GetItem(int id) { return itemList[id]; }

    public int GetItemQuantity(int id) { return quantinty[id]; }
    
    public int GetEnemyBuff(int id) { return enemyBuff[id]; }

    public int GetHeroBuff(int id) { return heroBuff[id]; }

    public void ChangeResetStatus() { toReset = true; }
}
