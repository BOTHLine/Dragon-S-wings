using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FallingCondition : MonoBehaviour
{
    private Entity entity;

    public int numIslandCollisions = 0;

    public bool entityShouldFall { get; private set; }

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();

        entityShouldFall = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numIslandCollisions++;
        entityShouldFall = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        numIslandCollisions--;
        if (numIslandCollisions == 0 && entity.CurrentStateAllowsFalling())
        {
            entityShouldFall = true;
        }
    }
}