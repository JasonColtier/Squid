using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{

    [SerializeField]
    private bool full360;

    [SerializeField]
    private float timeBetweenRotations;

    [SerializeField]
    private float rotationSpeed;

    void Start()
    {
        StartCoroutine(RotateMe());
    }

   

    private IEnumerator RotateMe()
    {
        float timeRot = 0;
        Vector3 startingRot = transform.rotation.eulerAngles;
        Vector3 targetRot = transform.eulerAngles + 180f * Vector3.forward;

        while (true)
        {
            if (full360)
            {
                transform.Rotate(0,0,rotationSpeed * Time.deltaTime);
            }
            else
            {
                if (transform.eulerAngles != targetRot)
                {
                    timeRot += Time.deltaTime;
                    transform.eulerAngles = Vector3.Lerp(startingRot, targetRot, rotationSpeed /10 * timeRot); // lerp to new angles
                }
                else
                {
                    timeRot = 0;
                    targetRot = startingRot;
                    startingRot = transform.eulerAngles;
                    yield return new WaitForSeconds(timeBetweenRotations);
                }
            }
            
            

            yield return new WaitForEndOfFrame();

        }
    }
}
