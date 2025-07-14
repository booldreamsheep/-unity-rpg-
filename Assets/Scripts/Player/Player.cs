using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class Player : Entity
{
    public bool inDialogue { get; private set; }

    [Header("Attack details")]
    
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    public bool isBusy { get;private set; }
    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaulMoveSpeed;
    private float defaulJumpForce;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaulDashSpeed;

    [Header("Climb Info")]
    public float climbSpeed=3f;
    public LayerMask ladderLayer;
    public LadderDetector ladderDetector;


    public float dashDir { get; private set; }
 
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
  
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSworrd { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    
    public PlayerBlackholeState blackHole { get; private set; }
    public PlayerDieStates dieStates { get; private set; }

    public PlayerClimbState climbState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide= new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSworrd = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole = new PlayerBlackholeState(this, stateMachine, "Jump");
        dieStates = new PlayerDieStates(this, stateMachine, "Die");
        climbState = new PlayerClimbState(this, stateMachine, "IsClimbing");
    }

    protected override void Start()
    {
       base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);

        defaulMoveSpeed = moveSpeed;
        defaulJumpForce = jumpForce;
        defaulDashSpeed = dashSpeed;
        
    }
    protected override void Update()
    {


        if(Time.timeScale == 0||inDialogue)
        {
            return;
        }

        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();


        if (Input.GetKeyDown(KeyCode.F)&&skill.crystal.crystalUnlocked)
        {
            skill.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }


    public void SetDialogueState(bool state)
    {
        inDialogue = state;
        if (state)
        {
            ZeroVelocity();
            isBusy = true; // 如果需要在对话时保持busy状态
        }
        else
        {
            isBusy = false;
        }
    }


    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce=jumpForce*(1 - _slowPercentage);
        dashSpeed= dashSpeed*(1 - _slowPercentage);
        anim.speed=anim.speed*(1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaulMoveSpeed;
        jumpForce = defaulJumpForce;
        dashSpeed = defaulDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

   
    public void ZeroVelocity()=>rb.velocity=new Vector2(0, 0);


    public IEnumerator  BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger()
    {
        
        stateMachine.currentState.AnimationFinishTrigger();
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }

        if (skill.dash.dashUnlocked == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)&&SkillManager.instance.dash.CanUseSkill())
        {
           
            dashDir =Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dieStates);
    }


    protected override void SetupZreoKmockbackPower()
    {
       knockbackPower=new Vector2(0, 0);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Climb"))
        {
            climbState._shouldExitLadder = true;
        }
    }

}

