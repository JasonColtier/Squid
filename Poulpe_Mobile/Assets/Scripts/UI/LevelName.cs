using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelName : MonoBehaviour
{

    private TextMeshProUGUI textLevelName;

    void Start()
    {
        string name = GameObject.Find("GameManager").GetComponent<GameManager>().levelName;
        textLevelName = GetComponent<TextMeshProUGUI>();
        string levelName = SceneManager.GetActiveScene().name;
        string text = levelName.Replace("Level_", "Niveau ");

        textLevelName.text = text + " : "+name;
    }
}
