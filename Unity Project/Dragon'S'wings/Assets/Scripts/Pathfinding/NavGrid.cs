using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public int[] gridSize = new int[2];
    public float distanceBetweenNotes;

    public float noteRadius;
    public float noteDistance;


    public NavMeshNote[,] grid;

    private void Awake()
    {


    }

    // Use this for initialization
    void Start()
    {

    }

    public void createGrid()
    {
        grid = new NavMeshNote[gridSize[0], gridSize[1]];


        for (int x = 0; x < gridSize[0]; x++)
        {
            for (int y = 0; y < gridSize[1]; y++)
            {



            }
        }
    }
}

