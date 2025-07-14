using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCountter {  get; private set; }
    private float lastTimeAttack;
    private float comboWindow = 2;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //AudioManager.instance.PlaySFX(2);//¹¥»÷ÒôÐ§

        xInput = 0;
        if(comboCountter>2||Time.time>= lastTimeAttack + comboWindow)
        {
            comboCountter = 0;
        }
        player.anim.SetInteger("ComboCounter", comboCountter);
        player.SetVelocity(player.attackMovement[comboCountter].x * player.facingDir, player.attackMovement[comboCountter].y);
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        comboCountter++;
        lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            player.ZeroVelocity();
        }
        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
