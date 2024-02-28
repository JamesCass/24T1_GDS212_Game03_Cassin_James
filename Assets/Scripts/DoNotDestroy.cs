using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    private void Awake()
    {
        // Register the method to be called when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Don't destroy the object on load if it's not in "GameScene"
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("BGM");

        Debug.Log("Number of BGM objects: " + musicObj.Length);
        Debug.Log("Current Scene: " + scene.name);

        // Destroy the object if there are more than 1 instances
        if (musicObj.Length > 1 || scene.name == "GameScene")
        {
            Destroy(this.gameObject);
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
