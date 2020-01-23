using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHead : MonoBehaviour
{
    public Vector3 originalPos; 
    private Vector3 originalPos1;
    private Vector3 originalPos2;
    public Transform target1;
    public Transform target2;

    public float factor;


    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        originalPos1 = target1.position;
        originalPos2 = target2.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, originalPos);
        Vector3 middle = (target1.position + target2.position) / 2;

        target1.position = Vector3.Lerp(originalPos1, middle, distance * factor);
        target2.position = Vector3.Lerp(originalPos2, middle, distance * factor);

        RotateTowardPoint(transform.position.x, transform.position.y);
    }

    //se tourne vers une direction / un point
    private void RotateTowardPoint(float x, float y)
    {
        x += originalPos.x;
        y += originalPos.y;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg ;

        transform.rotation = Quaternion.AngleAxis(angle, transform.forward);

        //transform.LookAt(originalPos, -Vector3.forward);

    }
}
