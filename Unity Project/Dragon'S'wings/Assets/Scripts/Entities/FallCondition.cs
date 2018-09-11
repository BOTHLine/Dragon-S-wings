using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class FallCondition : MonoBehaviour
{
    public Character character;

    public int numIslandCollisions = 0;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numIslandCollisions++;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        numIslandCollisions--;
    }

    public bool EntityShouldFall()
    {
        return numIslandCollisions <= 0;
    }
}