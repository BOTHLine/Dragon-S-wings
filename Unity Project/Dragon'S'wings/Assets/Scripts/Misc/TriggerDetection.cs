using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    [System.Serializable]
    public class MyCollider2DEvent : UnityEngine.Events.UnityEvent<Collider2D>
    {

    }

    public MyCollider2DEvent onTriggerEnterEvent;
    public MyCollider2DEvent onTriggerStayEvent;
    public MyCollider2DEvent onTriggerExitEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       onTriggerEnterEvent.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        onTriggerStayEvent.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExitEvent.Invoke(collision);
        Debug.Log("Trigger Exit");
    }
}
