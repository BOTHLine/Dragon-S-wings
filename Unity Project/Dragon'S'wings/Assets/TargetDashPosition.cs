using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TargetDashPosition : MonoBehaviour
{
    public int numCollisions = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numCollisions++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        numCollisions--;
    }
}