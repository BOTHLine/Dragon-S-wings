using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float spacing;
    public float maxDistance;
    private List<GameObject> dots;
    private int lastCount = 0;
    private GameObject cursor;

    public bool shrinkDots = false;
    private Vector3 startScaleDots;


    void Start()
    {
        startPoint = gameObject.transform.parent.position;
        dots = new List<GameObject>();
        cursor = (GameObject)Instantiate(Resources.Load("AimingDot"));
        startScaleDots = cursor.transform.localScale;
        cursor.transform.localScale = cursor.transform.localScale * 2;
    }


    // Update is called once per frame
    void Update()
    {
        //Cam setzt die z Position auf -10 ... das fixe ich hiermit: ++ new Vector3 (0,0,10)
        endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10); ;
        cursor.transform.position = endPoint;

        drawLine();
    }


    public void drawLine()
    {

        startPoint = gameObject.transform.parent.position;


        //Richtungsvektor für die Linie
        Vector2 diff = endPoint - startPoint;
        float distancePoints = Vector3.Distance(endPoint, startPoint);

        //Längencap
        if (distancePoints > maxDistance) distancePoints = maxDistance;

        //Anzahl an Dots
        int count = (int)(distancePoints / spacing);
        //Verschiebung um Sprünge beim spawnen von neuen Punkten zu verhinden
        float offset = (distancePoints) % spacing;


        while (dots.Count < count)
        {
            GameObject newDot = (GameObject)Instantiate(Resources.Load("AimingDot"));
            newDot.transform.parent = this.transform;
            dots.Add(newDot);
        }


        // Um in der Nächsten Runde Punkte die zu weit draußen sind wieder auszusortieren
        if (lastCount > count)
        {
            for (int i = lastCount - 1; i > count - 1; i--)
            {

                dots[i].transform.position = new Vector3(1000, 1000, 0);
                
            }
        }


        Vector3 step = (diff.normalized * spacing);

        for (int i = 0; i < count; i++)
        {
            GameObject currentDot = dots[i];
            currentDot.transform.position = startPoint + new Vector3(diff.x, diff.y, 0).normalized * offset + (step * i);

            if(shrinkDots) currentDot.transform.localScale = startScaleDots - startScaleDots*(0.05f*i);
        }

        // Um in der Nächsten Runde Punkte die zu weit draußen sind wieder auszusortieren
        lastCount = count;


    }

    //Hitdetection Coloring
    public void colorAllDots(Color newColor)
    {
        foreach (GameObject current in dots)
        {
            current.gameObject.GetComponent<SpriteRenderer>().color = newColor;
        }

        cursor.GetComponent<SpriteRenderer>().color = newColor;
    }


    public void resetColorOfDots()
    {
        colorAllDots(Color.white);
    }
}