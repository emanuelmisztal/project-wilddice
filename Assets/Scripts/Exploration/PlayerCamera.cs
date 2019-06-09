﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10.0f); // place camera above player
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10.0f); // move camera with player 
    }
}
