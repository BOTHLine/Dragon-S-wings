using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowingParable : MonoBehaviour
{
    public float height;
    public int arkSegmentsCount;

    public GameObject start;
    public GameObject end;


    private Vector3 startPoint;
    private Vector3 endPoint;

    private LineRenderer playerLine;
    private GameObject lineHolder;

    // Use this for initialization
    void Start ()
    {
        lineHolder = new GameObject("lineHolder");
        lineHolder.transform.parent = this.gameObject.transform;

        playerLine = lineHolder.AddComponent<LineRenderer>();
        playerLine.startWidth = 0.03f;
        playerLine.endWidth = 0.03f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        drawArk(start.transform.position, end.transform.position);



	}

    public void drawArk(Vector3 startPoint, Vector3 endPoint)
    {
        playerLine.positionCount = arkSegmentsCount+1;

        // f(x) = a(x-yend)²+h
        float steps = ((endPoint - startPoint).magnitude) / arkSegmentsCount;

        for(int i = 0; i <= arkSegmentsCount; i++)
        {
            // y = a (x - xs)² + ys | -ys
            // y - ys = a ( x - xs)² | /a
            // (y - ys) / a = (x - xs)² |Sqrt
            // Sqrt((y - ys) / a) = x - xs | + xs
            // Sqrt((y - ys) / a) + xs = x

            //float a = 0;
            //float x = Math.Sqrt((startPoint.y - height) / a) + 

            /*
            float x = i * steps;
            float a = (float)((endPoint.y - hight) / Math.Pow((endPoint.x - endPoint.y), 2f));
            
            float y = (float)(a * Math.Pow((x - endPoint.y), 2) + hight);

            playerLine.SetPosition(i, new Vector3(startPoint.x + i*steps,startPoint.y + y, -1));
            */


            Vector3 nextPoint = SampleParabola(startPoint, endPoint, height, i / (float) arkSegmentsCount);

            playerLine.SetPosition(i, new Vector3 (nextPoint.x,nextPoint.y, -1));






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
