using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightScript : MonoBehaviour 
{

	Light fireLight;
	float lightInt;
	public float min = 3f, max = 5f;

	public float range;
	private float startValue;


	// Use this for initialization
	void Start () 
	{
		fireLight = GetComponent<Light> ();
		startValue = fireLight.intensity;

	}
	
	// Update is called once per frame
	void Update () {
		lightInt = Random.Range (min, max);
		fireLight.intensity += lightInt;

		if (fireLight.intensity < startValue - range)
			fireLight.intensity = startValue - range;

		if (fireLight.intensity > startValue + range)
			fireLight.intensity = startValue + range;
	}
}
