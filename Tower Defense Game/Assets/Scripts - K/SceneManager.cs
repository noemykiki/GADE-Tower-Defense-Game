using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;  

public class SceneManager : MonoBehaviour
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
}
