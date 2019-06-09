/*
 * Author: Emanuel Misztal
 * 2019
 */

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
    private List<int> quantinty; // item quantity list
    private List<int> enemyBuff; // item enemy buff list
    private List<int> heroBuff; // item hero buff list
    private bool toReset = true; // flag to mark item list to be reloaded

    // when items button is clicked in battle
    public void OnItemsButtonClick()
    {
        gameObject.SetActive(true); // activate inventory list
        Camera.main.transform.position = new Vector3(0f, 13f, -1f); // move camera over inventory

        // if something changed in inventory
        if (toReset)
        {
            // reload lists
            itemList = new List<ItemStruct>();
            enemyBuff = new List<int>();
            heroBuff = new List<int>();
            quantinty = new List<int>();
            int i = 0; // reset item list index to 0

            // for every item and quantity pair in equipment
            foreach (KeyValuePair<ItemStruct, int> item in GameObject.FindObjectOfType<Equipment>().GetItemList())
            {
                itemList.Add(item.Key); // add item to list
                quantinty.Add(item.Value); // add quantity to item on list
                itemSprites[i].GetComponent<SpriteRenderer>().sprite = item.Key.sprite; // load item sprite
                itemDesc[i].text = item.Key.description + " X " + quantinty[i]; // load item description and quantity
                itemButtons[i].gameObject.SetActive(true); // activate button
                enemyBuff.Add(item.Key.enemyHP); // add enemy buff to item on list
                heroBuff.Add(item.Key.heroHP); // add hero buff to item on list
                i++; // increment item list index
            }

            toReset = false; // item list was reloaded so mark toReset flag to false
        }
    }

    // exit items button is clicked
    public void OnExitItemsButtonClick()
    {
        Camera.main.transform.position = new Vector3(0f, 0f, -1f); // move camera back to battle
        gameObject.SetActive(false); // deactivate item list
    }

    // interface for item
    public ItemStruct GetItem(int id) { return itemList[id]; } // return item struct

    // interface for item quantity
    public int GetItemQuantity(int id) { return quantinty[id]; } // return item quantity

    // interface for item enemy buf    
    public int GetEnemyBuff(int id) { return enemyBuff[id]; } // return item enemy buff

    // interface for hero buff
    public int GetHeroBuff(int id) { return heroBuff[id]; } // return item hero buff

    // change to reset status for item list
    public void ChangeResetStatus() { toReset = true; } // mark toReset flag to true so item list will be reset on next visit
}
