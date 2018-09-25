using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PolygonCollider2D))]
public class HitArea : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public bool playerInArea { get; private set; }

    private void Awake()
    {
        InitComponents();

        playerInArea = false;
    }

    private void InitComponents()
    {
        InitSpriteRenderer();
    }

    private void InitSpriteRenderer()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public void ToggleShow()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagList.Player)
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagList.Player)
        {
            playerInArea = false;
        }
    }
}