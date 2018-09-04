using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHighlighter : MonoBehaviour
{
    Renderer rend;

    public GameObject player;
    private GameObject cursor;
    public bool checkTrue = false;
    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<Renderer>();
        
        rend.material.SetColor("_Color", new Color(1, 1, 1, 0));

        
        cursor = player.transform.Find("Character").Find("Crosshair").gameObject;
        player = player.transform.Find("Character").gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (checkTrue)
        {
            checkTarget();
            checkTrue = false;
        }
	}

    public bool checkTarget()
    {
        Debug.DrawRay(new Vector2(player.transform.position.x, player.transform.position.y),((new Vector2(player.transform.position.x, player.transform.position.y) - (new Vector2(cursor.transform.position.x, cursor.transform.position.y))))* -1000000.0f, Color.white);
        Debug.Log("Schuss!");
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y), ((new Vector2(player.transform.position.x, player.transform.position.y) - (new Vector2(cursor.transform.position.x, cursor.transform.position.y)))), Mathf.Infinity);
        if (hit.collider != null)
        { Debug.Log("HIT!"); }
            return false;
    }


    public bool checkLOS()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(player.transform.position, cursor.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(player.transform.position, cursor.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(player.transform.position, cursor.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    


        return false;
    }

}
