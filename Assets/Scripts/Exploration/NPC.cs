/*
 * Author: Emanuel Misztal
 * 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public int npcHP; // npc health
    public int expGain; // exp gain for player when enemy is defeted

    // player exit position when battle is over
    public float exitPositionX;
    public float exitPositionY;
}
