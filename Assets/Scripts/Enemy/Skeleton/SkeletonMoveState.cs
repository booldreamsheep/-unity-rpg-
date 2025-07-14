using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(14, null);
    }

    public override void Exit()
    {


        base.Exit();
       
        AudioManager.instance.StopSFX(14);
            
        
    }

    public override void Update()
    {
        base.Update();

        

        enemy.SetVelocity(enemy.facingDir *enemy.moveSpeed,rb.velocity.y);

        if(enemy.IsWallDetected()||!enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
        
    }
}
