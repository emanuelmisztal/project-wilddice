/*
 * Author: Emanuel Misztal
 * 2019
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour, IComparable<Dice>
{
    public Battle battle; // link to battle script

    private bool isSelected; // flag is dice selected
    private int finalSide; // final side value that dice reads in the end of coroutine
    private Sprite[] diceSides; // array of dice sides sprites to load from Resources folder
    private SpriteRenderer rend; // link to sprite renderer to change sprites

    // Use this for initialization
    private void Start()
    {
        isSelected = false; // mark is selected flag as false

        rend = GetComponent<SpriteRenderer>(); // Assign Renderer component

        diceSides = Resources.LoadAll<Sprite>("DiceSides/"); // Load dice sides sprites to array from DiceSides subfolder of Resources folder
    }

    // interface for final side of dice
    public int GetFinalSide() { return finalSide; } // Return final dice side number

    // Roll dice - calls RollTheDice coroutine so You dont have to remember its name
    public void RollDice()
    {
        isSelected = false; // mark is selected flag as false
        StartCoroutine("RollTheDice"); // roll dice
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number
        // It needs to be assigned, let it be 0 initially
        int randomDiceSide = 0;

        finalSide = 0; // Reset finalSide

        // Loop to switch dice sides ramdomly
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = UnityEngine.Random.Range(0, 5); // Pick up random value from 0 to 5 (All inclusive)
            rend.sprite = diceSides[randomDiceSide]; // Set sprite to upper face of dice from array according to random value
            yield return new WaitForSeconds(0.05f); // Pause before next itteration
        }

        finalSide = randomDiceSide + 1; // Assigning final side to use this value later in game
    }

    // select dice event
    public void OnSelectButton()
    {
        if (battle.GetStage() == 1) // if battle is in first stage
        {
            if (isSelected == true) gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("DiceSides/d" + finalSide); // if dice is selected load normal sprite ?
            else gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Active/d" + finalSide + "-active"); // else load selected sprite ?

            isSelected = !isSelected; // change is selected status to oposite
        }
    }

    // interface for is selected flag
    public bool GetIsSelected() { return isSelected; } // returns isSelected status

    // dices comparator
    public bool Equals(Dice other) { return this.finalSide == other.finalSide ? true : false; } // compare final sides of dices

    // dices comparator
    public int CompareTo(Dice other)
    {
        if (this.finalSide < other.finalSide) return 1; // is bigger
        else if (this.finalSide == other.finalSide) return 0; // equals
        else return -1; // is smaller
    }
}
