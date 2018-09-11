using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public EntityPlayer entity { get; private set; }
    public Hook hook { get; private set; }

    public int health;

    private void Awake()
    {
        entity = GetComponentInChildren<EntityPlayer>();
        hook = GetComponentInChildren<Hook>();
    }

    public void Hit(int damage)
    {
        health -= damage;
        Debug.Log("Player got hit for " + damage + " damage.");
    }
}