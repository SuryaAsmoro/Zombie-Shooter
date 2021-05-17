using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Text speedText;

    private void Update()
    {
        speedText.text = "Speed: " + playerMovement.speed;
    }
}
