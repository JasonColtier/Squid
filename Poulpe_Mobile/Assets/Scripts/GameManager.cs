using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textJumpCount;

    [SerializeField]
    private TextMeshProUGUI collectibleCount;

    [SerializeField]
    private int bestJumpCount;

    public string levelName;

    private float delayWaitNextLevel = 4.5f ;

    private int currentJumpCount;

    private int numberOfCollectible;

    private int collected = 0;

    private SaveManager saveManager;

    public int GetBestJumpCount()
    {
        return bestJumpCount;
    }   
    
    public int GetCurrentJumpCount()
    {
        return currentJumpCount;
    }

    public int GetNumberOfCollectible()
    {
        return numberOfCollectible;
    }  
    
    public int GetCollected()
    {
        return collected;
    }



    // Start is called before the first frame update
    void Start()
    {
        numberOfCollectible = GameObject.FindGameObjectsWithTag("Collectible").Length;
        collectibleCount.text = "0 / " + numberOfCollectible;
        currentJumpCount = 0;
        textJumpCount.text = currentJumpCount + " / " + bestJumpCount;

        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();

    }

    public void UpdateUIJumpCount()
    {
        currentJumpCount++;
        textJumpCount.text = currentJumpCount + " / " + bestJumpCount;
    }

    public void RestartLevel()
    {
        Debug.Log("restart level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Collected()
    {
        collected++;
        collectibleCount.text = collected+" / " + numberOfCollectible;
    }

    public void Win()
    {
        Debug.Log("win");
        
        int sceneNumber = int.Parse(SceneManager.GetActiveScene().name.Replace("Level_", ""));

        bool meduses = false;
        if (collected == numberOfCollectible)
            meduses = true;

        bool movement = false;
        if (currentJumpCount <= bestJumpCount)
            movement = true;

        saveManager.UnlockLevel(sceneNumber, movement, meduses);

        StartCoroutine(WaitStartNextLevel(delayWaitNextLevel, sceneNumber));
    }

    private IEnumerator WaitStartNextLevel(float delay,int number)
    { 
        yield return new WaitForSeconds(delay);

        if (Application.CanStreamedLevelBeLoaded("Level_" + (number + 1)))
        {
            SceneManager.LoadScene("Level_" + (number + 1));
        }
        else
        {
            SceneManager.LoadScene("_MainMenu");
        }
    }
}
