using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHighlighter : MonoBehaviour
{
    Renderer rend;

    public GameObject player;
    private GameObject cursor;
    private Collider2D coll;
    public bool checkTrue = false;
    private Hook hook;
    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<Renderer>();
        
        rend.material.SetColor("_Color", new Color(1, 1, 1, 0));

        hook = player.transform.Find("Character").Find("Hook").gameObject.GetComponent<Hook>();
        Debug.Log(hook);
        cursor = player.transform.Find("Character").Find("Crosshair").gameObject;
        player = player.transform.Find("Character").gameObject;



        coll = GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
      
            if (checkTarget())
            {
                rend.material.SetColor("_Color", new Color(1, 1, 1, 1));
            }

            else
            {
                rend.material.SetColor("_Color", new Color(1, 1, 1, 0));
            }
            checkTrue = false;
        
	}

    public bool checkTarget()
    {
        Debug.Log(hook.maxRopeLength);
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, cursor.transform.position - player.transform.position, hook.maxRopeLength);
        if (hit.collider == coll)
            return true;
        return false;

    }


}
