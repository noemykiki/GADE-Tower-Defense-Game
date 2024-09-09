using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;  

public class SceneManagerScript : MonoBehaviour
{
    // This function will be called when the Quit button is pressed
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
    
    // This function will be called when the "Next" button is pressed
    public void LoadNextScene()
    {
        // Load the next scene in the build (based on the current scene index)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the build scenes
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes to load!");
        }
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
