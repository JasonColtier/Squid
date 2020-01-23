using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevelFromButton : MonoBehaviour
{
    public int number;
    
    [SerializeField]
    private bool isGameLevel;

    [SerializeField]
    private string levelName;

    public void StartLevel()
    {
        if (isGameLevel)
        {
            SceneManager.LoadScene("Level_" + number);
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
