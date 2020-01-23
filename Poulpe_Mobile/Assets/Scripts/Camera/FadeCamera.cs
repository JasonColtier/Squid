using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCamera : MonoBehaviour
{

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private Color fadeColor;

    [SerializeField]
    private float fadeTime;

    [SerializeField]
    private float delayStart;

    private float time;
   
    public void FadeToBlack()
    {
        //fadeImage.color = fadeColor;

        StartCoroutine(StartFadeToBlack(false,delayStart));

    }

    void Start()
    {
        StartCoroutine(StartFadeToBlack(true,0f));
    }



    private IEnumerator StartFadeToBlack(bool invert,float delay)
    {
        time = 0;
        float a = 0;

        yield return new WaitForSeconds(delay);

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            a = Mathf.Lerp(0, 1, time);

            Color col;
            if (invert)
            {
                col = new Color(fadeColor.r, fadeColor.g, fadeColor.b,1- a);
            }
            else
            {
                col = new Color(fadeColor.r, fadeColor.g, fadeColor.b, a);
            }
            fadeImage.color = col;
            yield return new WaitForEndOfFrame();
        }
    }
}
