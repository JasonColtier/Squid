using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGoBackToPlayer : MonoBehaviour
{

    public Button button;
    public Image image;

    public void CallCameraGoBackToPLayer()
    {
        Camera.main.GetComponent<MoveCamera>().GoBackToPlayer();
        HideButton();
    }

    public void HideButton()
    {
        button.enabled = false;
        image.enabled = false;
    }
}
