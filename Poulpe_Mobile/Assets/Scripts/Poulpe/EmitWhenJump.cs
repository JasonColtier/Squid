using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitWhenJump : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;

    public void StartEmit()
    {
        particleSystem.Play();
    }

    public void StopEmit()
    {
        particleSystem.Stop();
    }
}
