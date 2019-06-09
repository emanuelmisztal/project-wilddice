/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Dictionary<ItemStruct, int> items; // dictionary containg (item, quantity) pairs

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // mark this object not to be destroyed on changing level
        items = new Dictionary<ItemStruct, int>(); // create new dictionary of items and it's quantitys
    }

    // Add item to eq
    public void AddItem(ItemStruct item)
    {
        // check if item is already set in dictionary
        if (items.ContainsKey(item)) items[item] += 1; // add one to quantity of item
        else items.Add(item, 1); // create first item in dictionary with quantity of 1
    }

    // Remove item from eq
    public void RemoveItem(ItemStruct item)
    {
        if (items.ContainsKey(item) && items[item] > 0) items[item]--; // remove 1 item if it is in dictionary and it's quantity is greater than 0
    }

    // Get item list
    public Dictionary<ItemStruct, int> GetItemList() { return items; } // return dictionary of items

}
