﻿/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// structure of item
public struct ItemStruct
{
    public Sprite sprite; // place for item sprite
    public string name; // item name
    public string description; // item description
    public int enemyHP; // enemy buff
    public int heroHP; // hero buff

    // constructor
    public ItemStruct(Sprite _sprite, string _name, string _description, int _enemyHP, int _heroHP)
    {
        sprite = _sprite;
        name = _name;
        description = _description;
        enemyHP = _enemyHP;
        heroHP = _heroHP;
    }
}

public class Item : MonoBehaviour
{
    public string itemName; // name of item
    public int itemID; // id of item
    public string desc; // item's description

    // stats
    public int enemyHP; // what to do with enemy hp
    public int heroHP; // what to do with players hp

    private Sprite sprite; // place for sprite


    // get sprite at the begining
    private void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite; // get sprite from SpriteRenderer component
    }

    // copy constructor for item
    public Item(Item item)
    {
        itemName = item.itemName;
        itemID = item.itemID;
        desc = item.desc;
        enemyHP = item.enemyHP;
        heroHP = item.heroHP;
        sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    // to compare in eq dictionary
    public override string ToString() { return itemName; } // return item name

    // to compare in eq dictionary
    public override bool Equals(object other)
    {
        if (other.ToString() == this.ToString()) return true; // returns true if names match
        else return false; // returns false if names dont mach
    }

    // to compare in eq dictionary
    public override int GetHashCode() { return itemID; } // return item ID

    // get sprite
    public Sprite GetSprite() { return sprite; } // returns item sprite

    // return struct created from info about this item
    public ItemStruct ItemToStruct()
    {
        return new ItemStruct(gameObject.GetComponent<SpriteRenderer>().sprite, itemName, desc, enemyHP, heroHP); // as above
    }
}
