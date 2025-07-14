using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraoundState : PlayerState
{
   
    public PlayerGraoundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.R)&&player.skill.blackhole.blackHoleUnlockled)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
            {
                return;
            }

            stateMachine.ChangeState(player.blackHole);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)&&HasNoSworrd()&&player.skill.sword.swordUnlocked)
        {
            stateMachine.ChangeState(player.aimSworrd);
        }

        if (Input.GetKeyDown(KeyCode.Q)&&player.skill.parry.parryUnlocked)
        {
            stateMachine.ChangeState(player.counterAttack);
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttack);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }
       
        if (Input.GetKeyDown(KeyCode.Space)&&player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }


   
  



    private bool HasNoSworrd()
    {
        if (!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;


    }

}
