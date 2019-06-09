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
        isSelected = false;

        // Assign Renderer component
        rend = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
    }

    // Return final dice side number
    public int GetFinalSide()
    {
        return finalSide;
    }

    // Roll dice - calls RollTheDice coroutine so You dont have to remember its name
    public void RollDice()
    {
        isSelected = false;
        StartCoroutine("RollTheDice");
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number
        // It needs to be assigned, let it be 0 initially
        int randomDiceSide = 0;

        // Reset finalSide
        finalSide = 0;

        // Loop to switch dice sides ramdomly
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = UnityEngine.Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side to use this value later in game
        finalSide = randomDiceSide + 1;
    }

    // select dice event
    public void OnSelectButton()
    {
        if (battle.GetStage() == 1)
        {
            if (isSelected == true) gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("DiceSides/d" + finalSide);
            else gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Active/d" + finalSide + "-active");

            isSelected = !isSelected;
        }
    }

    // return isSelected
    public bool GetIsSelected()
    {
        return isSelected;
    }

    public bool Equals(Dice other) { return this.finalSide == other.finalSide ? true : false; }

    public int CompareTo(Dice other)
    {
        if (this.finalSide < other.finalSide) return 1;
        else if (this.finalSide == other.finalSide) return 0;
        else return -1;
    }
}
