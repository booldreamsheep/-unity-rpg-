using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackPower;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnockback;

    public int knockbackDir { get; private set; }
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public System.Action onFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr=GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        fx=GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd= GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage,float _slowDuration)
    {

    }
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact()
    {
        
        StartCoroutine("HitKnockback");
       // Debug.Log(gameObject.name+"Damager");
    }

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if(_damageDirection.position.x > transform.position.x)
        {
            knockbackDir = -1;   
        }
        else if (_damageDirection.position.x < transform.position.x)
        {
            knockbackDir = 1;
        }
       
    }

    public void SetupKnockbackPower(Vector2 _knockbackpower)=> knockbackPower = _knockbackpower;

    protected virtual IEnumerable HitKnockback()
    {
        isKnockback = true;
        rb.velocity = new Vector2(knockbackPower.x * -knockbackDir, knockbackPower.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
        SetupZreoKmockbackPower();
    }

    protected virtual void SetupZreoKmockbackPower()
    {

    }


    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);



    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
        {
          onFlipped();
        }
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion


    #region Velocity
    public void SetZeroVelocity()
    {
        if(isKnockback) return;


        rb.velocity =new  Vector2(0,0);
    }
 
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnockback) return;


        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    #endregion


    public virtual void Die()
    {

    }

}
