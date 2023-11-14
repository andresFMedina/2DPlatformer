using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockBackDirection;
    protected bool isKnocked;

    public float facingDirection { get; private set; } = 1f;
    public bool isFacingRight { get; private set; } = true;

    #region Components
    public Animator animator { get; set; }
    public Rigidbody2D rb { get; set; }

    public EntityFX entityFX { get; private set; }
    #endregion


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        entityFX = GetComponent<EntityFX>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) return;

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetZeroVelocity() 
    {
        if (isKnocked) return;

        rb.velocity = Vector2.zero; 
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180.0f, 0f);
    }

    public virtual void FlipController(float xVelocity)
    {
        if (xVelocity < 0f && isFacingRight) Flip();
        else if (xVelocity > 0f && !isFacingRight) Flip();
    }

    public virtual void Damage()
    {
        entityFX.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockBackDirection.x * -facingDirection, knockBackDirection.y);

        yield return new WaitForSeconds(0.7f);

        isKnocked = false;
    }
}
