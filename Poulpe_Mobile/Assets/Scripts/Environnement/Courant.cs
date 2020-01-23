using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Courant : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private Vector3 force;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb = collision.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        rb.AddForce(force);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        rb.AddForce(-force);
    }
}
