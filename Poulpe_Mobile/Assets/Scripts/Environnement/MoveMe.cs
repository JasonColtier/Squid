using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMe : MonoBehaviour
{
    [SerializeField]
    private float timeToDestination; 
    
    [SerializeField]
    private float waitAtDestination;

    [SerializeField]
    private float delayBeforeStart;

    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform endPoint;


    [SerializeField]
    private bool flipSprite;

    private SpriteRenderer spriteRenderer;

    private bool flipped;

    private bool waitOnce = false;

    void Start()
    {
        StartCoroutine(MoveTo(startPoint.position, endPoint.position, timeToDestination));

        if (flipSprite)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            flipped = false;
        }
    }

   

    private IEnumerator MoveTo(Vector3 posStart, Vector3 posEnd, float timeToDestination)
    {
        if(delayBeforeStart > 0 && waitOnce == false)
        {
            yield return new WaitForSeconds(delayBeforeStart);
            waitOnce = true;
        }

        float time = 0;

        while (true)
        {
            if (time < timeToDestination)
            {
                transform.position = Vector3.Lerp(posStart, posEnd, (time / timeToDestination));
                time += Time.deltaTime;
            }
            else if (time >= timeToDestination)
            {
                transform.position = posEnd;
                time = 0;
                yield return new WaitForSeconds(waitAtDestination);
                Vector3 tmp = posStart;
                posStart = posEnd;
                posEnd = tmp;
                if (flipSprite)
                {
                    flipped = !flipped;
                    spriteRenderer.flipX = flipped;
                }
            }
            
            
            yield return new WaitForEndOfFrame();

        }
    }
}
