using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int GetUnlockedLevelCount()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels");

        if (unlockedLevels == 0)
        {
            unlockedLevels = 1;
        }
        Debug.Log("unlocked levels " + unlockedLevels);
        return unlockedLevels;
    }

    public int GetLevelMovementScore(int level)
    {
        return PlayerPrefs.GetInt("Movement_level_"+ level);
    }

    public int GetLevelMedusesScore(int level)
    {
        return PlayerPrefs.GetInt("Meduses_level_" + level);
    }

    public void UnlockLevel(int levelJustFinished,bool movement,bool meduses)
    {
        
        if (movement)
        {
            PlayerPrefs.SetInt("Movement_level_"+levelJustFinished, 1);
        }
        if (meduses)
        {
            PlayerPrefs.SetInt("Meduses_level_" + levelJustFinished, 1);
        }

        if (GetUnlockedLevelCount() > levelJustFinished)
            return; // on fait pas la sauvegarde si les autres niveaux sont déja débloqués

        PlayerPrefs.SetInt("UnlockedLevels", levelJustFinished + 1);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }

}
