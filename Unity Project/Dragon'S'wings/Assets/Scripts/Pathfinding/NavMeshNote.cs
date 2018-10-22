using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshNote : MonoBehaviour
{
    public bool walkable;
    public Vector3 worldPosition;
    public int[] gridPosition= new int[2];

    public int gCost;
    public int hCost;

    private List<NavMeshNote> allWalkableNavMeshNotesInRange;
    private NavGrid myNavGrid;
    

	// Use this for initialization
	void Start ()
    {
		
	}
	

}
