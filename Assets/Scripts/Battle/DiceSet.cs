using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSet : MonoBehaviour
{
    public Dice[] diceSet; // set containg all 5 dices

    private int difficulty;
    private short outcome;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("difficulty")) PlayerPrefs.SetInt("difficulty", 0);
        difficulty = PlayerPrefs.GetInt("difficulty");
    }

    // Rolling whole set
    public void RollSet()
    {
        foreach (Dice dice in diceSet) dice.RollDice();
        outcome = CalculateOutcome();
    }

    // Reroll selected dices
    public void RerollSet()
    {
        foreach (Dice dice in diceSet) if (dice.GetIsSelected()) dice.RollDice();
        outcome = CalculateOutcome();
    }

    public short GetOutcome() { return outcome; }

    // Invoke this method when outcome is to be calculated
    public short CalculateOutcome()
    {
        // Table to store final sides
        short[] dices = new short[5];

        // Create copy of sides
        for (int i = 0; i < 5; i++) dices[i] = (short)diceSet[i].GetFinalSide();

        // Sort table
        System.Array.Sort(dices);

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

    public void AIreroll(short playerOutcome)
    {
        System.Array.Sort(diceSet);
        switch (difficulty)
        {
            case 0:
                if (playerOutcome > outcome)
                {
                    switch (outcome)
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
}
