using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour
{

    [Header("PHASE 1 déplacement joueur")]
    public List<GameObject> imagesPhase1 = new List<GameObject>();

    [Header("PHASE 2 déplacement camera")]

    [SerializeField]
    public List<GameObject> imagesPhase2 = new List<GameObject>();


    [Header("PHASE 3")]
    public List<GameObject> imagesPhase3 = new List<GameObject>();


    [Header("SCRIPTS")]
    public PlayerManager playerManager;

    public MoveCamera moveCamera;

    private bool showPhase1Once = false;
    private bool showPhase2Once = false;

    private void Start()
    {
        moveCamera.overridePanTuto = false;
    }

    public void HidePhase1()
    {
        if (!showPhase1Once)
        {
            foreach (GameObject g in imagesPhase1)
            {
                g.SetActive(false);
            }
            playerManager.overrideSelectionTuto = false;
            StartCoroutine(WaitPhase2(1));
            showPhase1Once = true;

        }
    }


    public void ShowPhase2()
    {
        if (!showPhase2Once)
        {
            foreach (GameObject g in imagesPhase2)
            {
                g.SetActive(true);
            }
            moveCamera.overridePanTuto = true;
            showPhase2Once = true;
        }
       
    }

    public void StartWaitEndPhase2()
    {
        StartCoroutine(WaitEndPhase2(3));
    }

    private void HidePhase2()
    {
        foreach (GameObject g in imagesPhase2)
        {
            g.SetActive(false);
        }
        playerManager.overrideSelectionTuto = true;
        ShowPhase3();
    }

    private void ShowPhase3()
    {
        foreach (GameObject g in imagesPhase3)
        {
            g.SetActive(true);
        }
    }

    private IEnumerator WaitEndPhase2(int sec)
    {
        yield return new WaitForSeconds(sec);
        HidePhase2();
    }

    private IEnumerator WaitPhase2(int sec)
    {
        yield return new WaitForSeconds(sec);
        ShowPhase2();
    }

    
}
