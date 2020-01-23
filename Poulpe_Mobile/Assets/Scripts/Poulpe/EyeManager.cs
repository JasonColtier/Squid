using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeManager : MonoBehaviour
{

    [SerializeField]
    private Sprite eye_Full;

    [SerializeField]
    private Sprite eye_Blink;

    [SerializeField]
    private Sprite eye_Bored;

    [SerializeField]
    private Sprite eye_Dead;

    [SerializeField]
    private Sprite eye_Happy;

    [SerializeField]
    private Sprite eye_Special;

    [SerializeField]
    private Sprite eye_Stretch;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Stretch()
    {
        spriteRenderer.sprite = eye_Stretch;
    }

    public void Full()
    {
        spriteRenderer.sprite = eye_Full;
    }

    public void Blink()
    {
        spriteRenderer.sprite = eye_Blink;
    }

    public void Happy()
    {
        spriteRenderer.sprite = eye_Happy;
    }

    public void Dead()
    {
        spriteRenderer.sprite = eye_Dead;
    }

    public void Special()
    {
        spriteRenderer.sprite = eye_Special;
    }

    public void Bored()
    {
        spriteRenderer.sprite = eye_Bored;
    }

}
