using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    public class AnchorPoint
    {
        public Vector2 position;
        public float ropeLength;
        public bool angle;

        public AnchorPoint(Vector2 position, Vector2 lastPosition, bool angle)
        {
            this.position = position;
            this.angle = angle;
            ropeLength = Vector2.Distance(position, lastPosition);

            Debug.Log("Rope Length: " + ropeLength);
        }

        public AnchorPoint(Vector2 position, bool angle)
        {
            this.position = position;
            this.angle = angle;
        }
    }

    private Player player;

    private LineRenderer lineRenderer;

    private List<AnchorPoint> anchorPoints = new List<AnchorPoint>();

    public int layerMask { get; private set; }

    public float maxRopeLength = 10.0f;
    public float availableRopeLength;
    public float currentRopeLength = 0.0f;

    public float distanceThreshold = 0.01f;

    private void Awake()
    {
        InitComponents();
        CreateLayerMask();
    }

    private void InitComponents()
    {
        player = GetComponentInParent<Player>();

        InitRigidbody2D();
        InitLineRenderer();
    }

    private void InitRigidbody2D()
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    private void InitLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
    }

    private void CreateLayerMask()
    {
        layerMask = 0;
        int layer = gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics2D.GetIgnoreLayerCollision(layer, i))
            {
                layerMask = layerMask | 1 << i;
            }
        }
    }

    private void Start()
    {
        transform.parent = player.character.transform;
    }

    private void FixedUpdate()
    {
        // Debug.Log("Amount of Points: " + anchorPoints.Count);
        if (anchorPoints.Count > 0)
        {
            HandleUnwrapNew();
            HandleWrap();
        }

        UpdateLineRenderer();
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = anchorPoints.Count + 1;
        for (int i = 0; i < anchorPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, anchorPoints[i].position);
        }
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
    }

    public void Shoot(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxRopeLength, layerMask);

        if (hit.collider)
        {
            Vector2 anchorPosition = hit.point + ((Vector2)transform.position - hit.point).normalized * distanceThreshold;
            anchorPoints.Add(new AnchorPoint(anchorPosition, false));
            availableRopeLength = ((Vector2)transform.position - anchorPosition).magnitude;
            player.character.distanceJoint2D.enabled = true;
            player.character.distanceJoint2D.distance = availableRopeLength;
            player.character.distanceJoint2D.connectedAnchor = GetLastAnchorPoint().position;
            player.character.SetActionState(Character.ActionState.Hooked);
        }
    }

    private void HandleUnwrapNew()
    {
        if (anchorPoints.Count <= 1)
            return;

        int anchorIndex = anchorPoints.Count - 2;
        int hingeIndex = anchorPoints.Count - 1;

        Vector2 anchorPosition = anchorPoints[anchorIndex].position;

        Vector2 hingePosition = anchorPoints[hingeIndex].position;
        Vector2 hingeDir = hingePosition - anchorPosition;

        Vector2 playerDir = (Vector2) transform.position - anchorPosition;

        Debug.DrawRay(anchorPosition, playerDir);
        Debug.DrawRay(anchorPosition, hingeDir);

        float angle = Vector2.SignedAngle(hingeDir, playerDir);
        if (angle > 0.0f)
        {
            if (anchorPoints[hingeIndex].angle)
            {
                RemoveLastAnchorPoint();
            }
        } else if (angle < 0.0f)
        {
            if (!anchorPoints[hingeIndex].angle)
            {
                RemoveLastAnchorPoint();
            }
        }
    }
    
    private void HandleUnwrapOld()
    {
        if (anchorPoints.Count <= 1)
            return;

        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, anchorPoints[anchorPoints.Count - 2].position - (Vector2)transform.position, (anchorPoints[anchorPoints.Count - 2].position - (Vector2)transform.position).magnitude, layerMask);
        if (!raycastHit2D.collider)
        {
            RemoveLastAnchorPoint();
        }
    }

    private void HandleWrap()
    {
        if (anchorPoints.Count == 0)
            return;

        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetLastAnchorPoint().position - (Vector2)transform.position, (GetLastAnchorPoint().position - (Vector2)transform.position).magnitude, layerMask);
        if (raycastHit2D.collider)
        {
            Vector2 wrapPoint = GetClosestColliderPointFromRaycastHit2D(raycastHit2D);
            Vector2 wrapPosition = wrapPoint + ((Vector2) transform.position - wrapPoint).normalized * distanceThreshold;
            if (wrapPosition.Equals(GetLastAnchorPoint().position))
                return;

            Vector2 anchorPosition = GetLastAnchorPoint().position;
            Vector2 hingeDir = wrapPosition - anchorPosition;

            Vector2 playerDir = (Vector2)transform.position - anchorPosition;


            anchorPoints.Add(new AnchorPoint(wrapPosition, anchorPoints[anchorPoints.Count - 1].position, Vector2.SignedAngle(hingeDir, playerDir) < 0.0f));
            currentRopeLength += anchorPoints[anchorPoints.Count - 1].ropeLength;

            player.character.distanceJoint2D.distance = availableRopeLength - currentRopeLength;
            player.character.distanceJoint2D.connectedAnchor = GetLastAnchorPoint().position;
        }
    }

    private Vector2 GetClosestColliderPointFromRaycastHit2D(RaycastHit2D hit)
    {
        PolygonCollider2D coll = (PolygonCollider2D)hit.collider;
        Debug.Log(hit.collider);

        Vector2[] points = new Vector2[coll.points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = (Vector2)coll.gameObject.transform.position + Vector2.Scale(coll.points[i], coll.gameObject.transform.lossyScale);
        }

        Vector2 closestPoint = points[0];
        for (int i = 1; i < points.Length; i++)
        {
            if ((points[i] - hit.point).magnitude < (closestPoint - hit.point).magnitude)
            {
                closestPoint = points[i];
            }
        }
        return closestPoint;
    }

    public void ResetAnchorPoints()
    {
        anchorPoints.Clear();
        lineRenderer.positionCount = 0;
        player.character.distanceJoint2D.enabled = false;
        currentRopeLength = 0.0f;
    }

    public AnchorPoint GetLastAnchorPoint()
    {
        return anchorPoints[anchorPoints.Count - 1];
    }

    public void RemoveLastAnchorPoint()
    {
        float lastAddedRopeLength = anchorPoints[anchorPoints.Count - 1].ropeLength;
        anchorPoints.RemoveAt(anchorPoints.Count - 1);
        
        if (anchorPoints.Count > 0)
        {
            currentRopeLength -= lastAddedRopeLength;
            player.character.distanceJoint2D.distance = availableRopeLength - currentRopeLength;
            player.character.distanceJoint2D.connectedAnchor = GetLastAnchorPoint().position;
        }
        else
        {
            player.character.SetActionState(Character.ActionState.Falling);
        }
    }
}