using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
