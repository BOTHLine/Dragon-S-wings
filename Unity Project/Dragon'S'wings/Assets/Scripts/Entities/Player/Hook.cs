using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    [System.Serializable]
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
        }

        public AnchorPoint(Vector2 position, bool angle)
        {
            this.position = position;
            this.angle = angle;
        }
    }

    public Entity entity;
    public HookState hookState;
    public DistanceJoint2D distanceJoint2D;

    public LineRenderer lineRenderer;

    public List<AnchorPoint> anchorPoints = new List<AnchorPoint>();

    public int layerMask { get; private set; }
    
    public float availableRopeLength;
    public float currentRopeLength = 0.0f;

    public float distanceThreshold = 0.01f;

    private void Awake()
    {
        InitComponents();
        CreateLayerMask();
    }

    public void InitComponents()
    {
        entity = GetComponentInParent<Entity>();
        hookState = entity.GetComponent<HookState>();
        distanceJoint2D = entity.gameObject.AddComponent<DistanceJoint2D>();

        distanceJoint2D.enableCollision = true;
        distanceJoint2D.autoConfigureDistance = false;
        distanceJoint2D.maxDistanceOnly = true;
        distanceJoint2D.enabled = false;

        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
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

    private void FixedUpdate()
    {
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, hookState.maxRopeLength, layerMask);
        if (hit.collider)
        {
            Vector2 anchorPosition = hit.point + ((Vector2)entity.transform.position - hit.point).normalized * distanceThreshold;
            anchorPoints.Add(new AnchorPoint(anchorPosition, false));
            availableRopeLength = ((Vector2)entity.transform.position - anchorPosition).magnitude;
            distanceJoint2D.enabled = true;
            distanceJoint2D.distance = availableRopeLength;
            distanceJoint2D.connectedAnchor = GetLastAnchorPoint().position;
            entity.SetActionState(new HookStateParameter());
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


            anchorPoints.Add(new AnchorPoint(wrapPosition, GetLastAnchorPoint().position, Vector2.SignedAngle(hingeDir, playerDir) < 0.0f));
            currentRopeLength += GetLastAnchorPoint().ropeLength;

            distanceJoint2D.distance = availableRopeLength - currentRopeLength;
            distanceJoint2D.connectedAnchor = GetLastAnchorPoint().position;
        }
    }

    private Vector2 GetClosestColliderPointFromRaycastHit2D(RaycastHit2D hit)
    {
        PolygonCollider2D coll = (PolygonCollider2D)hit.collider;

        Vector2[] points = new Vector2[coll.points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = (Vector2)coll.gameObject.transform.position + Vector2.Scale(coll.points[i], coll.gameObject.transform.lossyScale);
        }

        Vector2 closestPoint = points[0];
        for (int i = 1; i < points.Length; i++)
        {
            if ((points[i] - hit.point).sqrMagnitude < (closestPoint - hit.point).sqrMagnitude)
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
        distanceJoint2D.enabled = false;
        currentRopeLength = 0.0f;
        entity.SetActionState(new FallStateParameter(new MovementStateParameter()));
    }

    public AnchorPoint GetLastAnchorPoint()
    {
        if (anchorPoints.Count == 0)
            return null;
        return anchorPoints[anchorPoints.Count - 1];
    }

    public void RemoveLastAnchorPoint()
    {
        Debug.Log(anchorPoints.Count);
        float lastAddedRopeLength = GetLastAnchorPoint().ropeLength;
        anchorPoints.RemoveAt(anchorPoints.Count - 1);
        Debug.Log(anchorPoints.Count);
        
        if (anchorPoints.Count > 0)
        {
            currentRopeLength -= lastAddedRopeLength;
            distanceJoint2D.distance = availableRopeLength - currentRopeLength;
            distanceJoint2D.connectedAnchor = GetLastAnchorPoint().position;
        }
        else
        {
            ResetAnchorPoints();
        }
    }
}