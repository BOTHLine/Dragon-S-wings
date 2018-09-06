using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FallingCondition : MonoBehaviour
{
    private Entity entity;

    public int numIslandCollisions = 0;

    public bool entetiyShouldFall { get; private set; }

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();

        entetiyShouldFall = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numIslandCollisions++;
        entetiyShouldFall = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        numIslandCollisions--;
        if (numIslandCollisions == 0 && (character.currentActionState == Character.ActionState.Free || character.currentActionState == Character.ActionState.Falling) )
        {
            playerShouldFall = true;
        }
    }
}