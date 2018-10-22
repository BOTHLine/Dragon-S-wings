using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAnimationMod : MonoBehaviour 
{
	public float startFrame;

	private Animator anim;
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();

		StartCoroutine(WaitAtStart());	
	}
	
	// Update is called once per frame


	IEnumerator WaitAtStart()
	{
		anim.speed = 0;
		yield return new WaitForSeconds(startFrame);
		anim.speed = 0.5f;
	}

}
