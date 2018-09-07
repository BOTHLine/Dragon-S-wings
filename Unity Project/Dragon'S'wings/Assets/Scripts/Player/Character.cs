using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DistanceJoint2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Character : Entity
{
    public enum ActionState
    {
        Free,
        Falling,
        Dashing,
        Hooked,
        Swinging,
        Pulling,
        Repositioning
    }

    private Player player;

    public new Rigidbody2D rigidbody2D { get; private set; }
    public DistanceJoint2D distanceJoint2D { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    private Transform crosshair;

    [SerializeField]
    public ActionState currentActionState;

    private Vector2 movingDirection = Vector2.zero;
    public float movementSpeed = 5.0f;

    private Vector2 aimingDirection = Vector2.down;
    public float aimRadius = 2.0f;

    public float fallTime = 0.5f;
    private float timeFalling;

    private Vector2 facingDirection = Vector2.down;
    private Vector2 dashingDirection = Vector2.zero;
    private Vector2 repositioningDirection = Vector2.zero;
    private bool canDash = true;
    public float dashSpeed = 10.0f;
    public float dashDistance = 2.0f;
    private float dashTime;
    private float timeDashing;
    public float dashForce = 5.0f;

    private Vector2 lastSavePosition;

    public float pullSpeed = 5.0f;
    public float pullVelocity = 0.0f;
    public float pullMaxSpeed = 0.0f;
    // public float maxPullSpeed = 100.0f;
    private float pullThreshold = 0.5f;

    public float swingSpeed = 10.0f;
    public float swingVelocity = 0.0f;
    public float swingMaxSpeed = 0.0f;
    private Vector2 swingRelativePosition;
    private bool swingClockwise;
    public float swingForce = 10.0f;

    public float repositioningSpeed;
    public float repositioningDistance;
    public float repositioningTime;
    public float timeRepositioning;
    
    private FallingCondition fallingCondition;
    private FallingSave fallingSave;

    private void Awake()
    {
        InitComponents();

        crosshair = transform.Find("Crosshair").transform;

        dashTime = dashDistance / dashSpeed;

        currentActionState = ActionState.Free;
    }

    private void InitComponents()
    {
        InitRigidBody2D();
        InitDistanceJoint2D();

        player = GetComponentInParent<Player>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        fallingCondition = GetComponentInChildren<FallingCondition>();
        fallingSave = transform.Find("FallingSave").GetComponent<FallingSave>();
    }

    private void InitRigidBody2D()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody2D.freezeRotation = true;
        if (rigidbody2D.drag == 0.0f)
            rigidbody2D.drag = 1.0f;
    }

    private void InitDistanceJoint2D()
    {
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        distanceJoint2D.enableCollision = true;
        distanceJoint2D.autoConfigureDistance = false;
        distanceJoint2D.maxDistanceOnly = true;
        distanceJoint2D.enabled = false;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleActions();
    }

    private void HandleInput()
    {
        HandleAimInput();
        HandleMovementInput();
        switch (currentActionState)
        {
            case ActionState.Free:
                HandleDashInput();
                HandleHookInput();
                break;
            case ActionState.Falling:
                HandleDashInput();
                break;
            case ActionState.Dashing:
                break;
            case ActionState.Hooked:
                HandleReleaseInput();
                HandlePullInput();
                HandleSwingInput();
                break;
            case ActionState.Pulling:
                HandleReleaseInput();
                break;
            case ActionState.Swinging:
                HandleReleaseInput();
                break;
        }
    }

    private void HandleActions()
    {
        switch (currentActionState)
        {
            case ActionState.Free:
                HandleMovementAction();
                break;
            case ActionState.Falling:
                HandleFallingAction();
                break;
            case ActionState.Dashing:
                HandleDashAction();
                break;
            case ActionState.Hooked:
                HandleMovementAction();
                break;
            case ActionState.Pulling:
                HandlePullAction();
                break;
            case ActionState.Swinging:
                HandleSwingAction();
                break;
            case ActionState.Repositioning:
                HandleRepositioningAction();
                break;
        }
    }

    public void SetActionState(ActionState newActionState)
    {
        switch (currentActionState)
        {
            case ActionState.Free:
                lastSavePosition = transform.position;
                break;
            case ActionState.Falling:
                timeFalling = 0.0f;
                fallingSave.gameObject.SetActive(false);
                break;
            case ActionState.Hooked:
                lastSavePosition = transform.position;
                break;
        }

        switch (newActionState)
        {
            case ActionState.Free:
                gameObject.layer = LayerList.Player;
                player.hook.ResetAnchorPoints();
                canDash = true;
                break;
            case ActionState.Falling:
                player.hook.ResetAnchorPoints();
                gameObject.layer = LayerList.PlayerFalling;
                rigidbody2D.velocity = Vector2.zero;
                fallingSave.gameObject.SetActive(true);
                break;
            case ActionState.Dashing:
                gameObject.layer = LayerList.PlayerDashing;
                canDash = false;
                timeDashing = 0.0f;
                dashingDirection = aimingDirection.normalized;
                Trailer.AddTrailer(spriteRenderer, dashTime, 0.05f, 1.0f, 10.0f, 0.1f);
                break;
            case ActionState.Swinging:
                gameObject.layer = LayerList.PlayerDashing;
                break;
            case ActionState.Pulling:
                gameObject.layer = LayerList.PlayerDashing;
                break;
            case ActionState.Repositioning:
                gameObject.layer = LayerList.PlayerDashing;
                canDash = false;
                timeRepositioning = 0.0f;
                break;
        }

        currentActionState = newActionState;
    }

    private void HandleAimInput()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
        Vector3 aimDirection = worldMousePosition - transform.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);
        if (aimAngle < 0.0f)
            aimAngle = Mathf.PI * 2 + aimAngle;

        aimingDirection = Quaternion.Euler(0.0f, 0.0f, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        crosshair.localPosition = (Vector3) aimingDirection.normalized * aimRadius;
    }

    private void HandleMovementInput()
    {
        movingDirection = Vector2.zero;

        movingDirection.x = Input.GetAxisRaw(InputList.Horizontal);
        movingDirection.y = Input.GetAxisRaw(InputList.Vertical);

        facingDirection = movingDirection != Vector2.zero ? movingDirection : facingDirection;
    }

    private void HandleDashInput()
    {
        if (canDash && Input.GetButtonDown(InputList.Dash))
            SetActionState(ActionState.Dashing);
    }

    private void HandleHookInput()
    {
        if (Input.GetButtonDown(InputList.Hook))
            player.hook.Shoot(crosshair.localPosition);
    }

    private void HandleReleaseInput()
    {
        if (Input.GetButtonDown(InputList.Release))
            SetActionState(ActionState.Free);
    }

    private void HandlePullInput()
    {
        if (!Input.GetButton(InputList.Hook))
            SetActionState(ActionState.Pulling);
    }

    private void HandleSwingInput()
    {
        if (Input.GetButton(InputList.Swing))
        {
            swingRelativePosition = (Vector2)transform.position - distanceJoint2D.connectedAnchor;
            if (Mathf.Abs(movingDirection.x) > Mathf.Abs(movingDirection.y))
            {
                if (swingRelativePosition.y != 0)
                {
                    swingClockwise = (swingRelativePosition.y > 0 && movingDirection.x > 0) || (swingRelativePosition.y < 0 && movingDirection.x < 0);
                }
                else
                {
                    swingClockwise = (swingRelativePosition.x > 0 && movingDirection.y < 0) || (swingRelativePosition.x < 0 && movingDirection.y > 0);
                }
                SetActionState(ActionState.Swinging);
            }
            else if (Mathf.Abs(movingDirection.y) > Mathf.Abs(movingDirection.x))
            {
                if (swingRelativePosition.x != 0)
                {
                    swingClockwise = (swingRelativePosition.x > 0 && movingDirection.y < 0) || (swingRelativePosition.x < 0 && movingDirection.y > 0);
                }
                else
                {
                    swingClockwise = (swingRelativePosition.y > 0 && movingDirection.x > 0) || (swingRelativePosition.y < 0 && movingDirection.x < 0);
                }
                SetActionState(ActionState.Swinging);
            }
        }
    }

    private void HandleMovementAction()
    {
        rigidbody2D.velocity = movingDirection.normalized * movementSpeed;
    }

    private void HandleFallingAction()
    {
        if (fallingCondition.numIslandCollisions > 0)
        {
            SetActionState(ActionState.Free);
            return;
        }

        timeFalling += Time.fixedDeltaTime;
        if (timeFalling >= fallTime)
        {
            Respawn();
        }
    }

    private void HandleDashAction()
    {
        rigidbody2D.velocity = dashingDirection * dashSpeed;

        timeDashing += Time.fixedDeltaTime;
        if (timeDashing >= dashTime)
        {
            rigidbody2D.velocity = Vector2.zero;
            SetActionState(ActionState.Falling);
        }
    }

    public void StartRepositioning(float speed, Vector2 distanceVector)
    {
        repositioningDistance = distanceVector.magnitude;
        repositioningDirection = distanceVector.normalized;
        repositioningSpeed = speed;
        repositioningTime = repositioningDistance / repositioningSpeed;
        SetActionState(ActionState.Repositioning);
    }

    private void HandleRepositioningAction()
    {
        rigidbody2D.velocity = repositioningDirection * repositioningSpeed;

        timeRepositioning += Time.fixedDeltaTime;
        if (timeRepositioning >= repositioningTime)
        {
            rigidbody2D.velocity = Vector2.zero;
            SetActionState(ActionState.Falling);
        }
    }

    private void HandlePullAction()
    {
        if (Input.GetButton(InputList.Hook))
        {
            SetActionState(ActionState.Falling);
            return;
        }
        if ((distanceJoint2D.connectedAnchor - (Vector2) transform.position).magnitude <= pullThreshold)
        {
            player.hook.RemoveLastAnchorPoint();
        }

        Vector2 forceDirection = distanceJoint2D.connectedAnchor - (Vector2) transform.position;
        if (pullVelocity == 0.0f)
        {
            rigidbody2D.velocity = forceDirection.normalized * pullSpeed;
        } else
        {
            rigidbody2D.AddForce(forceDirection.normalized * pullVelocity);
            if (rigidbody2D.velocity.magnitude > pullSpeed)
            {
                rigidbody2D.velocity = rigidbody2D.velocity.normalized * pullSpeed;
            }
        }

        float maxDistance = ((Vector2) transform.position - distanceJoint2D.connectedAnchor).magnitude;
        if (maxDistance < distanceJoint2D.distance)
        {
            distanceJoint2D.distance = maxDistance;
        }
    }

    private void HandleSwingAction()
    {
        if (!Input.GetButton(InputList.Swing))
        {
            SetActionState(ActionState.Falling);
            return;
        }
        Vector2 midVector = distanceJoint2D.connectedAnchor - (Vector2) transform.position;
        Vector2 swingDirection = swingClockwise ? new Vector2(-midVector.y, midVector.x) : new Vector2(midVector.y, -midVector.x);
        Debug.DrawRay(transform.position, swingDirection);
        distanceJoint2D.distance = midVector.magnitude;
        if (swingVelocity == 0.0f)
        {
            rigidbody2D.velocity = swingDirection.normalized * swingSpeed;
        }
        else
        {
            rigidbody2D.AddForce(swingDirection.normalized * swingVelocity);
            if (rigidbody2D.velocity.magnitude > swingSpeed)
            {
                rigidbody2D.velocity = rigidbody2D.velocity.normalized * swingSpeed;
            }
        }
        //rigidbody2D.velocity = swingDirection.normalized * swingSpeed;
    }

    public void Respawn()
    {
        transform.position = lastSavePosition;
        SetActionState(ActionState.Free);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerList.Level)
        {
            //numIslandCollisions++;
        }
    }
    */

    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerList.Level)
        {
            //numIslandCollisions--;
        }
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (currentActionState)
        {
            case ActionState.Swinging:
                if (collision.gameObject.tag == TagList.Enemy)
                {
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                    Vector2 vectorToEnemyPosition = enemy.transform.position - transform.position;
                    enemy.rigidbody2D.velocity = vectorToEnemyPosition.normalized * rigidbody2D.velocity.magnitude;
                    StartRepositioning(rigidbody2D.velocity.magnitude, vectorToEnemyPosition);

                    enemy.Push();
                }
                break;
            case ActionState.Dashing:
                if (collision.gameObject.tag == TagList.Enemy)
                {
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                    Vector2 vectorToEnemyPosition = enemy.transform.position - transform.position;
                    enemy.rigidbody2D.velocity = vectorToEnemyPosition.normalized * rigidbody2D.velocity.magnitude;
                    StartRepositioning(dashSpeed, vectorToEnemyPosition);

                    enemy.Push();
                }
                break;
        }
    }

    public override bool CurrentStateAllowsFalling()
    {
        return (currentActionState == ActionState.Free || currentActionState == ActionState.Falling);
    }
}