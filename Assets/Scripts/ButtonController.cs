using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    private void Start()
    {
        Config.CreateScoreFile();
    }
    public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void ResetGameSettings()
    {
        GameSettings.Instance.ResetGameSettings();
    }
    

}
