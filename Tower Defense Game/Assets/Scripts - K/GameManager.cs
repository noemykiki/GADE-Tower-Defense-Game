using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script deals with basic game admin tasks
public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    
    public void TogglePause() //Handles Pausing the game
    {
        if (isPaused)
        {
            // Resume the game
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            // Pause the game
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    

}
