using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

//This script deals with basic game admin tasks
public class GameManager : MonoBehaviour
{
    public TMP_Text rewardText;
    public TMP_Text CastleHealth;
    private bool isPaused = false;
    public GameObject gameOverScreen;
    public EnemySpawn enemySpawn;


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
            Time.timeScale = 0f;
            gameOverScreen.SetActive(true);
        }
    }
    

    private void Start()
    {
        // Initialize or start the enemy spawning when the game starts
        enemySpawn.StartSpawning();
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Stop existing coroutines and restart enemy spawning
        enemySpawn.StopSpawning();
        enemySpawn.StartSpawning();
        Time.timeScale = 1f;
    }

}
