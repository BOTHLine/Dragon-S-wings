using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public enum ActionState
    {
        Idling,
        Chasing,
        Attacking,
        WaitingForAttack,
        Staggered
    }

    private Blinker blinker;

    private AttackRange attackRange;
    private HitArea hitArea;

    public new Rigidbody2D rigidbody2D { get; private set; }
    private SpriteRenderer spriteRender;

    public ActionState currentActionState;
    private static Player player;

    public float moveSpeed;

    public float chaseRange;

    public float waitingTime;
    private float timeWaited = 0.0f;

    public float attackTime;
    private float timeAttacked = 0.0f;

    public float staggerBaseTime;
    private float staggerTime;
    private float timeStaggered = 0.0f;

    public float staggerImmuneBaseTime;
    private float staggerImmuneTime;
    private float timeStaggerImmune = 0.0f;

    public int damage;

    private void Awake()
    {
        InitComponents();

        currentActionState = ActionState.Idling;
    }

    private void InitComponents()
    {
        InitRigidbody2D();

        player = FindObjectOfType<Player>();

        attackRange = GetComponentInChildren<AttackRange>();
        hitArea = GetComponentInChildren<HitArea>();
    }

    private void InitRigidbody2D()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.freezeRotation = true;
        if (rigidbody2D.drag == 0.0f)
            rigidbody2D.drag = 1.0f;
    }

    private void FixedUpdate()
    {
        HandleActions();
    }

    private void HandleActions()
    {
        Vector2 vectorToPlayer = player.character.transform.position - transform.position;
        
        if (timeWaited < waitingTime)
            timeWaited += Time.fixedDeltaTime;
        if (timeStaggerImmune < staggerImmuneTime)
            timeStaggerImmune += Time.fixedDeltaTime;

        switch (currentActionState)
        {
            case ActionState.Idling:
                if (vectorToPlayer.magnitude <= chaseRange)
                {
                    SetActionState(ActionState.Chasing);
                }
                break;
            case ActionState.Chasing:
                if (vectorToPlayer.magnitude > chaseRange)
                {
                    SetActionState(ActionState.Idling);
                }
                else if (attackRange.playerInRange)
                {
                    SetActionState(ActionState.WaitingForAttack);
                }
                else
                {
                    MoveTowardsPlayer();
                }
                break;
            case ActionState.WaitingForAttack:
                {
                    if (!attackRange.playerInRange)
                    {
                        SetActionState(ActionState.Chasing);
                    }
                    else
                    {
                        LookAtPlayer();
                        if (timeWaited >= waitingTime)
                            SetActionState(ActionState.Attacking);
                    }
                }
                break;
            case ActionState.Attacking:
                if (timeAttacked < attackTime)
                {
                    timeAttacked += Time.fixedDeltaTime;
                }
                else
                {
                    Attack();
                    SetActionState(ActionState.Chasing);
                }
                break;
           
            case ActionState.Staggered:
                if (timeStaggered < staggerTime)
                {
                    timeStaggered += Time.fixedDeltaTime;
                }
                else
                {
                    SetActionState(ActionState.Chasing);
                }
                break;
        }
    }

    public void SetActionState(ActionState newActionState)
    {
        switch (currentActionState)
        {
            case ActionState.Chasing:
                rigidbody2D.velocity = Vector2.zero;
                break;
            case ActionState.Attacking:
                hitArea.ToggleShow();
                break;
        }

        switch (newActionState)
        {
            case ActionState.Idling:
                break;
            case ActionState.Chasing:
                break;
            case ActionState.WaitingForAttack:
                break;
            case ActionState.Attacking:
                timeWaited = 0.0f;
                timeAttacked = 0.0f;
                hitArea.ToggleShow();
                break;
            case ActionState.Staggered:
                timeStaggered = 0.0f;
                break;
        }

        currentActionState = newActionState;
    }

    private void LookAtPlayer()
    {
        Vector2 direction = player.character.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void MoveTowardsPlayer()
    {
        LookAtPlayer();
        rigidbody2D.AddForce((player.character.transform.position - transform.position).normalized * moveSpeed * rigidbody2D.drag);
    }

    private void Attack()
    {
        if (hitArea.playerInArea)
            player.Hit(damage);
    }

    public void Hit(float staggerMultiplier = 1.0f)
    {
        if (timeStaggerImmune >= staggerImmuneTime)
        {
            staggerTime = staggerBaseTime * staggerMultiplier;
            staggerImmuneTime = staggerImmuneBaseTime * staggerMultiplier;

            SetActionState(ActionState.Staggered);
        }
    }
}