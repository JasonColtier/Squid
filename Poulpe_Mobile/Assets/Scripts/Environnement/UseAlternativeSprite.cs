using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAlternativeSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite alternativeSprite;

    private SpriteRenderer spriteRenderer;

    private Sprite originalSprite;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    public void UseAltenativeSprite()
    {
        spriteRenderer.sprite = alternativeSprite;
    }

    public void UseOriginalSprite()
    {
        spriteRenderer.sprite = originalSprite;
    }
}
