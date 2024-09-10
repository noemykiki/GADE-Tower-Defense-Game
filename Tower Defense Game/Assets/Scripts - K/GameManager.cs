using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script deals with basic game admin tasks
public class GameManager : MonoBehaviour
{
    public TMP_Text rewardText;
    public TMP_Text CastleHealth;
    private bool isPaused = false;
    public GameObject gameOverScreen;

    private void Start()
    {
        gameOverScreen.SetActive(false);
    }
    private void Update()
    {
        UpdateRewardUI();
        UpdateCastleHealth();
    }
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
    private void UpdateRewardUI()
    {
       
            rewardText.text = Enemy.totalReward.ToString();
    
    }

    private void UpdateCastleHealth()
    {
        CastleHealth.text = Castle.healthLeft.ToString();
        
        if (Castle.healthLeft <= 0)
        {
            gameOverScreen.SetActive(true);
        }
    }


}
