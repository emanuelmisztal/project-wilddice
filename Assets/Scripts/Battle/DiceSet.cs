/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSet : MonoBehaviour
{
    public Dice[] diceSet; // set containg all 5 dices

    private int difficulty; // difficulity level
    private short outcome; // dice set outcome

    private void Start()
    {
        if (!PlayerPrefs.HasKey("difficulty")) PlayerPrefs.SetInt("difficulty", 0); // set difficulity if none was set previously
        difficulty = PlayerPrefs.GetInt("difficulty"); // get difficulity level
    }

    // Rolling whole set
    public void RollSet()
    {
        // for every dice in dice set
        foreach (Dice dice in diceSet) dice.RollDice(); // roll dice
        outcome = CalculateOutcome(); // calculate dice set outcome
    }

    // Reroll selected dices
    public void RerollSet()
    {
        // for every selected dice in dice set
        foreach (Dice dice in diceSet) if (dice.GetIsSelected()) dice.RollDice(); // roll dice
        outcome = CalculateOutcome(); // calculate new outcome
    }

    // outcome interface
    public short GetOutcome() { return outcome; } // returns dice set outcome

    // Invoke this method when outcome is to be calculated
    public short CalculateOutcome()
    {
        short[] dices = new short[5]; // Table to store final sides

        for (int i = 0; i < 5; i++) dices[i] = (short)diceSet[i].GetFinalSide(); // Create copy of sides

        System.Array.Sort(dices); // Sort table

        // Count how many different sides are present
        short difCount = 0;
        for (int i = 1; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (dices[j] == i)
                {
                    difCount++;
                    break;
                }
                else continue;
            }
        }

        // Chose outcome code
        switch (difCount)
        {
            case 1:
                return 100; // Five in a row

            case 2:
                if ((dices[0] == dices[1] && dices[3] != dices[4]) || (dices[0] != dices[1] && dices[3] == dices[4])) return 70; // Four in a row
                else return 60; // Full

            case 3:
                short counter = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (dices[i] == dices[i + 1]) counter++;
                    else
                    {
                        counter = 0;
                        continue;
                    }
                    if (counter == 2) return 30; // Three in a row
                }
                return 20; // Two pairs

            case 4:
                return 10; // Pair

            case 5:
                if (dices[0] == dices[1] - 1 && dices[3] == dices[4] - 1) return 50; // Big Straight
                else return 40; // Small Straight

            default: return 0; // nothing
        }
    }

    // now it's time for enemy to reroll dices
    public void AIreroll(short playerOutcome)
    {
        outcome = CalculateOutcome();
        System.Array.Sort(diceSet); // sort dice set


        // what to do next depends on difficulity level
        switch (difficulty)
        {
            // easy mode
            case 0:
                // check if player outcome is higher than enemy outcome
                if (playerOutcome >= outcome)
                {
                    // what to reroll depends on what is in the set
                    switch (outcome) // switch on outcome code
                    {
                        case 0:
                            diceSet[(int)UnityEngine.Random.Range(0f, 4.1f)].RollDice();
                            break;

                        case 100:
                            break;

                        case 70:
                            if (diceSet[0].GetFinalSide() == diceSet[1].GetFinalSide()) diceSet[4].RollDice();
                            else diceSet[0].RollDice();
                            break;

                        case 60:
                            if (diceSet[1].GetFinalSide() == diceSet[2].GetFinalSide()) diceSet[3].RollDice();
                            else diceSet[2].RollDice();
                            break;

                        case 50:
                            break;

                        case 40:
                            if (diceSet[0].GetFinalSide() + 1 == diceSet[1].GetFinalSide()) diceSet[4].RollDice();
                            else diceSet[0].RollDice();
                            break;

                        case 30:
                            if (diceSet[2].GetFinalSide() != diceSet[3].GetFinalSide()) diceSet[3].RollDice();
                            else if (diceSet[0].GetFinalSide() != diceSet[1].GetFinalSide()) diceSet[0].RollDice();
                            else diceSet[1].RollDice();
                            break;

                        case 20:
                            if (diceSet[3] != diceSet[4]) diceSet[4].RollDice();
                            else if (diceSet[1] != diceSet[2]) diceSet[2].RollDice();
                            else diceSet[0].RollDice();
                            break;

                        case 10:
                            if (diceSet[0].GetFinalSide() == diceSet[1].GetFinalSide()) diceSet[2].RollDice();
                            else diceSet[0].RollDice();
                            break;
                    }
                }
                break;

            case 1:
                break;

            case 2:
                break;
        }
    }
}

/*
 // now it's time for enemy to reroll dices
    public void AIreroll(short playerOutcome)
    {
        System.Array.Sort(diceSet); // sort dice set

        // what to do next depends on difficulity level
        switch (difficulty)
        {
            // easy mode
            case 0:
                // check if player outcome is higher than enemy outcome
                if (playerOutcome > outcome)
                {
                    // what to reroll depends on what is in the set
                    switch (outcome) // switch on outcome code
                    {
                        case 0:
                            diceSet[0].RollDice();
                            break;

                        case 1:
                            break;

                        case 2:
                            if (diceSet[0].GetFinalSide() == diceSet[1].GetFinalSide()) diceSet[4].RollDice();
                            else diceSet[0].RollDice();
                            break;

                        case 3:
                            if (diceSet[1].GetFinalSide() == diceSet[2].GetFinalSide()) diceSet[3].RollDice();
                            else diceSet[2].RollDice();
                            break;

                        case 4:
                            break;

                        case 5:
                            if (diceSet[0].GetFinalSide() + 1 == diceSet[1].GetFinalSide()) diceSet[4].RollDice();
                            else diceSet[0].RollDice();
                            break;

                        case 6:
                            if (diceSet[2].GetFinalSide() != diceSet[3].GetFinalSide()) diceSet[3].RollDice();
                            else if (diceSet[0].GetFinalSide() != diceSet[1].GetFinalSide()) diceSet[0].RollDice();
                            else diceSet[1].RollDice();
                            break;

                        case 7:
                            if (diceSet[3] != diceSet[4]) diceSet[4].RollDice();
                            else if (diceSet[1] != diceSet[2]) diceSet[2].RollDice();
                            else diceSet[0].RollDice();
                            break;

                        case 8:
                            if (diceSet[0].GetFinalSide() == diceSet[1].GetFinalSide()) diceSet[2].RollDice();
                            else diceSet[0].RollDice();
                            break;
                    }
                }
                break;

            case 1:
                break;

            case 2:
                break;
        }
    }
 */
