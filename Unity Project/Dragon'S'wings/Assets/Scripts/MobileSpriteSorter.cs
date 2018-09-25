using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileSpriteSorter : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;
    // Use this for initialization
    void Start ()
    {
        mySpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        mySpriteRenderer.sortingOrder = -(int)(this.transform.position.y * 100);
    }
}
