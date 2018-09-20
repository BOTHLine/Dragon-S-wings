using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMyBox : MonoBehaviour
{
    public float height;
    public float flyTime;

    public GameObject shadow;

    private Vector3 startPoint;  
    private Vector3 targetPosition;
    private Vector2 startPosi;
    private Vector2 targetPosi;

    private Vector3 shadowOriginalScale;
    private Vector3 currentScale;

    public bool throwNow = false;
    private bool flying = false;
    private float flyCounter = 0f;
    // Use this for initialization
    void Start ()
    {
        shadowOriginalScale = shadow.transform.lossyScale;
	}
	


	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("space") && !throwNow && !flying)
        {
            throwNow = true;    
        }


        if (throwNow && !flying)
        {
            flyCounter = 0f;

            targetPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition) + new Vector3 (0,0,9); //Vector plus um z = -10 der cam auszugleichen
            startPoint = this.transform.position;
            

            targetPosi = new Vector2(targetPosition.x, targetPosition.y);
            startPosi = new Vector2(startPoint.x, startPoint.y);

            flying = true;
            throwNow = false;
        }

        if (flying)
        {
            flyTheBox();
            scaleTheShadow();

        }


	}

    private void flyTheBox()
    {
        Vector2 current = SampleParabola(startPosi, targetPosi, height, flyCounter / flyTime);
        this.transform.position = new Vector3 (current.x, current.y, -1);

        flyCounter++;

        shadow.transform.position = Vector3.Lerp(startPoint, targetPosition, flyCounter / flyTime);

        //shadow.transform.localScale *= ;

        if ((transform.position - targetPosition).magnitude <0.1)
        {
            shadow.transform.position = transform.position;
            shadow.transform.localScale = shadowOriginalScale;
            flying = false;
        }

    }

    public void scaleTheShadow()
    {
        float fracJourney = flyCounter / flyTime;

        if (fracJourney <= 0.5)
        {
            shadow.transform.localScale = shadowOriginalScale - shadowOriginalScale * (fracJourney);
            currentScale = transform.localScale;
        }
        else
        {
            shadow.transform.localScale = shadowOriginalScale * (fracJourney);
        }

    }


    Vector2 SampleParabola(Vector2 start, Vector2 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector2 travelDirection = end - start;
            Vector2 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector2 travelDirection = end - start;
            Vector2 levelDirecteion = end - new Vector2(start.x, end.y);
            Vector2 up = new Vector2(0.0f, 1.0f);
            //if (end.y > start.y) up = -up;
            Vector2 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up;
            return result;
        }
    }




}
