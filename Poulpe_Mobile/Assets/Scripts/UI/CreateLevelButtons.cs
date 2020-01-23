using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CreateLevelButtons : MonoBehaviour
{
    [SerializeField]
    private SaveManager saveManager;

    [SerializeField]
    private Transform canvasTransform;

    [SerializeField]
    private GameObject gameLevelButton;

    [SerializeField]
    private int numberOfLevels;

    private int numberOfUnlockedLevels;

    [SerializeField]
    private float distanceX;

    [SerializeField]
    private float distanceY;


    [SerializeField]
    private float startX;

    [SerializeField]
    private float startY;

    [SerializeField]
    private int numberPerRow;

    private float numberOfRow = 1;
    private float distanceBetweenRows = 0;

    void Start()
    {
        numberOfUnlockedLevels = saveManager.GetUnlockedLevelCount();

        StartCreation();
    }

    public void StartCreation()
    {
        

        

        for (int i = 1; i < numberOfLevels +1; i++)
        {
            bool movement = false;
            bool meduses = false;

            if (saveManager.GetLevelMovementScore(i) == 1) {
                movement = true;
            }
            if (saveManager.GetLevelMedusesScore(i) == 1)
            {
                meduses = true;
            }

            //Debug.Log("level " + i + " movement : " + saveManager.GetLevelMovementScore(i) + " meduses : " + saveManager.GetLevelMedusesScore(i));




            CreateLevelButton(i,movement,meduses);
        }
    }

    private void CreateLevelButton(int number, bool movementOK, bool medusesOK)
    {
        float posX = startX + distanceX * ((number-1)%numberPerRow);
        float posY = startY - distanceBetweenRows;
        GameObject monButton = Instantiate(gameLevelButton, canvasTransform);
        monButton.GetComponent<RectTransform>().localPosition = new Vector2(posX, posY);
        monButton.transform.GetChild(1).gameObject.SetActive(movementOK);
        monButton.transform.GetChild(2).gameObject.SetActive(medusesOK);
        monButton.GetComponentInChildren<TextMeshProUGUI>().SetText((number).ToString());
        monButton.GetComponent<StartLevelFromButton>().number = number;

        if(number > numberOfUnlockedLevels )
        {
            monButton.GetComponent<Button>().interactable = false;
        }

        if (number%numberPerRow == 0)
        {
            distanceBetweenRows = numberOfRow * distanceY;
            numberOfRow++;
        }

    }

}