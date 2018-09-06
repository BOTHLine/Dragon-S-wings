using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TreeHighlighter : MonoBehaviour
{
    private new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start ()
    {
        SetHighlight(false);
    }

    public void SetHighlight(bool newState)
    {
        renderer.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, newState ? 1.0f : 0.0f));
    }
}