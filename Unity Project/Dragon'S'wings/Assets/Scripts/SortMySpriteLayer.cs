using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortMySpriteLayer : MonoBehaviour
{

    private SpriteRenderer mySpriteRenderer;
    
    // Use this for initialization
	void Start ()
    {
        mySpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        mySpriteRenderer.sortingOrder = -(int) (this.transform.position.y * 100);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
