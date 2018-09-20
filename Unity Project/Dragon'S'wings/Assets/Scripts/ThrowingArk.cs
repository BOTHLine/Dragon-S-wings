using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowingArk : MonoBehaviour
{
    public bool useMe = false;
    private bool initialisiert = false;

    public GameObject player;


    public float arkFromPlayer;

    private Vector3 myPosi;
    private Vector3 playerPosi;

    private LineRenderer playerLine;
    private LineRenderer circleLine;
    private LineRenderer arcLine;

    private GameObject lineHolder;
    private GameObject circleHolder;
    private GameObject arcHolder;
    //private LineRenderer circle;

    // Use this for initialization
    void Start ()
    {
        lineHolder = new GameObject("lineHolder");
        lineHolder.transform.parent = this.gameObject.transform;

        circleHolder = new GameObject("circleHolder");
        circleHolder.transform.parent = this.gameObject.transform;

        arcHolder = new GameObject("arcHolder");
        arcHolder.transform.parent = this.gameObject.transform;

        playerLine = lineHolder.AddComponent<LineRenderer>();
        playerLine.startWidth = 0.03f;
        playerLine.endWidth = 0.03f;

        circleLine = circleHolder.AddComponent<LineRenderer>();
        circleLine.startWidth = 0.03f;
        circleLine.endWidth = 0.03f;

        arcLine = arcHolder.AddComponent<LineRenderer>();
        arcLine.startWidth = 0.03f;
        arcLine.endWidth = 0.03f;
        //playerline = new Material(Shader.Find("Particles/Additive"));

        //circle = gameObject.AddComponent<LineRenderer>();
        //circle = new Material(Shader.Find("Particles/Additive"));


    }

    // Update is called once per frame
    void Update ()
    {
        if (useMe)
        {
            updateMe();           
        }

        else
        {          
            clearMe();           
        }

	}

    public void updateMe()
    {
        myPosi = this.gameObject.transform.position + new  Vector3 (0, 0,-1);
        playerPosi = player.transform.position + new Vector3 (0, 0,-1);
        

        //playerline.transform.parent = this.gameObject.transform;

        playerLine.positionCount = 3;
        //playerline.useWorldSpace = false;

        playerLine.SetPosition(0, myPosi);
        playerLine.SetPosition(1, playerPosi);
        playerLine.SetPosition(2, myPosi + (playerPosi - myPosi) * 2);

        float radius = (playerPosi - myPosi).magnitude;
        drawCircle(radius);
        drawArc();
    }

    public void clearMe()
    {
        playerLine.positionCount = 0;
        circleLine.positionCount = 0;
    }

    public void drawArc()
    {
        arcLine.positionCount = 5;

        arcLine.SetPosition(0, myPosi);


        Vector3 direct = getArkCorners(-arkFromPlayer);       
        arcLine.SetPosition(1, direct);


        arcLine.SetPosition(2, playerPosi);

        direct = getArkCorners(arkFromPlayer);
        arcLine.SetPosition(3, direct);

        arcLine.SetPosition(4, myPosi);



    }

    public void drawCircle(float radius)
    {

        int segments = 40;

        float yradius = radius;
        float xradius = radius;


        circleLine.positionCount = (segments+1);

        float x;
        float y;


        float change = 2 * (float)Math.PI / segments;
        float angle = change;

        x = Mathf.Sin(0) * xradius;
        circleLine.SetPosition(0, (playerPosi + new Vector3(x, Mathf.Cos(0) * yradius, 0)));

        for (int i = 1; i < (segments +1); i++)
        {
            x = Mathf.Sin(angle) * xradius;
            y = Mathf.Cos(angle) * yradius;

            float drawSpeed = 0; // wenn der Kreis nach und nach gemalt werden sollte, hier eine andere Zahl eintragen

            //yield return new WaitForSeconds(drawSpeed);
            circleLine.SetPosition((int)i, (playerPosi + new Vector3(x, y,  0)));

            angle += change;
        }
    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public Vector3 getArkCorners(float ancle)
    {
        Vector3 direct = playerPosi - myPosi;
        Vector2 myDirect = Rotate(new Vector2(direct.x, direct.y), ancle);
        return playerPosi + new Vector3(myDirect.x, myDirect.y, 0);
    }
}
