using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsManager : MonoBehaviour
{
    private Text t;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        score = PlayerPrefs.GetInt("BestScore");
    }

    // Update is called once per frame
    void Update()
    {
        score++;
        t.text = "" + score;

        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }

    private void OnApplicationQuit()
    {
        //SAUVEGARDE
    }

    private void OnApplicationFocus(bool focus)
    {
        //SAUVEGARDE AUSSI :)
    }
}
