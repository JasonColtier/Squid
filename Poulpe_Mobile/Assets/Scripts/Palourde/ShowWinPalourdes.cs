using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWinPalourdes : MonoBehaviour
{
    [SerializeField]
    private GameObject palourdeMovement;

    [SerializeField]
    private GameObject palourdeMeduses;

    GameManager gameManager;

    private int bestJump;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bestJump = gameManager.GetBestJumpCount();
    }

    public void CalculateJumps()
    {
        if(gameManager.GetCurrentJumpCount() <= bestJump)
        {
            StartCoroutine(WaitForEndAnimHeart(1f));
        }
        else
        {
            StartCoroutine(CalculateMeduses(1f));
        }

        
    }

    public IEnumerator WaitForEndAnimHeart(float time)
    {
        yield return new WaitForSeconds(time);
        palourdeMovement.SetActive(true);
        StartCoroutine(CalculateMeduses(1f));
    }

    public IEnumerator CalculateMeduses(float time)
    {
        yield return new WaitForSeconds(time);
        if (gameManager.GetCollected() == gameManager.GetNumberOfCollectible() && gameManager.GetNumberOfCollectible() != 0)
        {
            palourdeMeduses.SetActive(true);
        }
    }
}
