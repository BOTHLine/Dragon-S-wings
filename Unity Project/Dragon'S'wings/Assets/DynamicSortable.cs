using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicSortable : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Sortable";
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = (int)(-transform.position.y * 100);
    }
}