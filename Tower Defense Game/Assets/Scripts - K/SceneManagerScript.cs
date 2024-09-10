using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;  

public class SceneManagerScript : MonoBehaviour
{
    public GameObject enemySpawnObject; // Reference to the GameObject with EnemySpawn script

    private Coroutine enemySpawnCoroutine;
    public void QuitGame()
    {
        #if UNITY_EDITOR
        // If running in the editor, stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If running as a standalone application, quit the application
            Application.Quit();
        #endif
    }


    public void LoadSampleScene()
    {
        // Load the SampleScene
        Castle.mainHealth = 500;
        Enemy.totalReward = 100;
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;


    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
      
    }
    public void RestartGame()
    {
         // Ensure that the coroutine restarts
        if (enemySpawnObject != null)
        {
            EnemySpawn enemySpawn = enemySpawnObject.GetComponent<EnemySpawn>();
            if (enemySpawn != null)
            {
                // Stop any existing coroutine
                

                // Start the coroutine again
                enemySpawnCoroutine = StartCoroutine(enemySpawn.ISpawnEnemies());
            }
        }
    }

}
