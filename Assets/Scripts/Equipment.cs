using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Dictionary<ItemStruct, int> items; // dictionary containg (item, quantity) pairs

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        items = new Dictionary<ItemStruct, int>();
    }

    // Add item to eq
    public void AddItem(ItemStruct item)
    {
        if (items.ContainsKey(item)) items[item] += 1;
        else items.Add(item, 1);
        Debug.Log(item + ": " + items[item] + " in eq");
    }

    // Remove item from eq
    public void RemoveItem(ItemStruct item)
    {
        if (items.ContainsKey(item) && items[item] > 0) items[item]--;
    }

    // Get item list
    public Dictionary<ItemStruct, int> GetItemList()
    {
        return items;
    }

}
