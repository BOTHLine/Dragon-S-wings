using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public float mass;
    public GameObject player;
    


    public bool useSlerp = false;
    public bool fakeThrowingArk = false;
    public bool ignoreHitBox = false;
    public bool throwMeNow = false;
    private bool flying = false;

    public float targetThreshhold;

    private Vector3 oldPosition;
    private Vector3 destination;

    private Vector3 oldScale;
    private Vector3 currentMaxScale;


    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;




    // Use this for initialization
    void Start ()
    {
        oldScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (throwMeNow)
        {
            throwMeNow = false;
            throwMe();
        }

        if (flying)
        {
            // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.



            if (useSlerp)
            {
                transform.position = Vector3.Slerp(oldPosition, destination, fracJourney);
               
            }

            else
            {
                transform.position = Vector3.Lerp(oldPosition, destination, fracJourney);
                           

                if (fakeThrowingArk)
                {

                    //Höhenfake durch y verschiebung + schatten
                                        

                    if (fracJourney <= 0.5)
                    {
                        transform.localScale = oldScale + oldScale * fracJourney;
                        currentMaxScale = transform.localScale;
                    }
                    else
                    {
                        transform.localScale = currentMaxScale + currentMaxScale * (1 - fracJourney);
                    }
                }
            }



            if ((transform.position - destination).magnitude <= targetThreshhold)
            {
                flying = false;
                transform.localScale = oldScale;
                transform.GetComponent<BoxCollider2D>().enabled = true;

            }
        }

	}

    public void throwMe()
    {
        oldPosition = transform.position;
        destination = oldPosition + (player.transform.position - oldPosition) * 2;

        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(oldPosition, destination);

        if (ignoreHitBox) transform.GetComponent<BoxCollider2D>().enabled = false;

        flying = true;
    }
}
