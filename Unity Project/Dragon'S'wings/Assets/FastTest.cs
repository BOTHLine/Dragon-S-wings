using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTest : MonoBehaviour {

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    public int currentIndex;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
        {
            currentIndex++;
            if (currentIndex >= sprites.Length)
                currentIndex = 0;
            spriteRenderer.sprite = sprites[currentIndex];
        }
	}
}
