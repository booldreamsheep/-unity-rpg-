using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerState
{
    private float _originalGravity;
    public bool _shouldExitLadder;

    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 保存原始重力并禁用重力
        _originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        // 重置速度
        rb.velocity = Vector2.zero;

        // 初始化状态参数
        _shouldExitLadder = false;

        // 设置动画参数
        player.anim.speed = 1f;
        player.anim.Play("PlayerClimbing");
    }

    public override void Exit()
    {
        base.Exit();

        // 恢复原始重力
        rb.gravityScale = _originalGravity;

        // 重置动画速度
        player.anim.speed = 1f;
    }

    public override void Update()
    {
        base.Update();

        // 优先处理状态退出
        if (_shouldExitLadder)
        {
            HandleStateTransition();
            return;
        }

        HandleVerticalMovement();
        HandleHorizontalMovement();
        CheckForExitConditions();

        if (Input.GetKey(KeyCode.Space)) {

            stateMachine.ChangeState(player.jumpState);
        }

    }

    private void HandleVerticalMovement()
    {
        yInput = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(rb.velocity.x, yInput * player.climbSpeed);

        // 设置动画参数
        player.anim.SetBool("IsClimbing", Mathf.Abs(yInput) > 0.1f);
    }

    private void HandleHorizontalMovement()
    {
        // 限制水平移动速度
        float xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * player.moveSpeed * 0.5f, rb.velocity.y);
    }

    private void CheckForExitConditions()
    {
        // 检测是否应该退出爬梯状态
        if (!player.ladderDetector.IsTouchingLadder || player.IsGroundDetected())
        {
            _shouldExitLadder = true;
        }
    }

    private void HandleStateTransition()
    {
        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

   
}
