using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Remove HP from bar by moving it from camera view
    public void RemoveHP(float multiplier)
    {
        this.transform.Translate(new Vector3(multiplier * -1.0f, 0f, 0f)); // -0.653817f
    }

    // Add HP
    // TO IMPLEMENT -> can be incremented only to max hp value
    public void AddHP(float multiplier)
    {
        this.transform.Translate(new Vector3(multiplier, 0f, 0f)); // 0.0056355f 
    }
}
