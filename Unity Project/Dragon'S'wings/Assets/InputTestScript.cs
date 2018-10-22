using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTestScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 330; i < 350; i++)
        {
            string inputButton = "JoystickButton" + i;
            if (Input.GetKeyDown((KeyCode) i))
            {
                Debug.Log("Button " + inputButton + " has been pressed");
            }
        }
    }
}