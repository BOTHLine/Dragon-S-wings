using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{

    public GameObject playerGameObject;
    private Transform target;

    private Camera myCam;
    private Vector2 screenSize;
    private float camZCoord;


    public float smoothing;

    public Vector3 camOffset;
    public Vector3 upperLeftBoarder;
    public Vector3 lowerRightBoarder;

    private LineRenderer boarderLine;

    void Start()
    {
        camZCoord = transform.position.z;

        myCam = this.gameObject.transform.GetComponent<Camera>();
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        target = playerGameObject.transform;

        boarderLine = this.gameObject.AddComponent<LineRenderer>();
        boarderLine.startWidth = 0.03f;
        boarderLine.endWidth = 0.03f;
        boarderLine.positionCount = 5;

        // camOffset = this.transform.position;
    }

    void FixedUpdate() //evtl doch lieber LateUpdate?
    {
        moveCam();
        drawMyBoarders();
    }


    public void moveCam()
    {
        Vector3 myPosi = this.transform.position;

        Debug.Log("myPosi: " + myPosi);

        Vector3 desiredPosition = target.position + camOffset;

        Debug.Log("Target: " + desiredPosition);
        Debug.Log("Screensize: " + screenSize);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, checkWorldBoarders(desiredPosition), smoothing * Time.deltaTime);

        transform.position = smoothedPosition;
    }


    public Vector3 checkWorldBoarders(Vector3 myPosi)
    {
       

        //Grenze Oben links
        if (myPosi.x - screenSize.x / 2 < upperLeftBoarder.x) myPosi.x = upperLeftBoarder.x + screenSize.x / 2;
        if (myPosi.y - screenSize.y / 2 < upperLeftBoarder.y) myPosi.y = upperLeftBoarder.y + screenSize.y / 2;

        //Grenze unten rechts
        if (myPosi.x + screenSize.x / 2 > upperLeftBoarder.x) myPosi.x = upperLeftBoarder.x + screenSize.x / 2;
        if (myPosi.y + screenSize.y / 2 < upperLeftBoarder.y) myPosi.y = upperLeftBoarder.y + screenSize.y / 2;

        myPosi = new Vector3(myPosi.x, myPosi.y, camZCoord);



        return myPosi;
    }


    private void drawMyBoarders()
    {


        boarderLine.SetPosition(0, upperLeftBoarder);
        boarderLine.SetPosition(1, new Vector2(lowerRightBoarder.x, upperLeftBoarder.y));
        boarderLine.SetPosition(2, lowerRightBoarder);
        boarderLine.SetPosition(3, new Vector2(upperLeftBoarder.x, lowerRightBoarder.y));
        boarderLine.SetPosition(4, upperLeftBoarder);

    }


}
