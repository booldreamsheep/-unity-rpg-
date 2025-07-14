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

        // ����ԭʼ��������������
        _originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        // �����ٶ�
        rb.velocity = Vector2.zero;

        // ��ʼ��״̬����
        _shouldExitLadder = false;

        // ���ö�������
        player.anim.speed = 1f;
        player.anim.Play("PlayerClimbing");
    }

    public override void Exit()
    {
        base.Exit();

        // �ָ�ԭʼ����
        rb.gravityScale = _originalGravity;

        // ���ö����ٶ�
        player.anim.speed = 1f;
    }

    public override void Update()
    {
        base.Update();

        // ���ȴ���״̬�˳�
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

        // ���ö�������
        player.anim.SetBool("IsClimbing", Mathf.Abs(yInput) > 0.1f);
    }

    private void HandleHorizontalMovement()
    {
        // ����ˮƽ�ƶ��ٶ�
        float xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * player.moveSpeed * 0.5f, rb.velocity.y);
    }

    private void CheckForExitConditions()
    {
        // ����Ƿ�Ӧ���˳�����״̬
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
