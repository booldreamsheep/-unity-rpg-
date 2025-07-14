using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;
    public int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName , EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Update()
    {
        base.Update();
        if(enemy.IsPlayerDetected())
        {
            stateTimer=enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                   stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            stateMachine.ChangeState(enemy.idleState);
        }


        if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }

        

        enemy.SetVelocity(moveDir * enemy.moveSpeed, rb.velocity.y);

    }

    public override void Exit()
    {
        base.Exit();
    }
    private bool CanAttack()
    {
        if ((Time.time>=enemy.lastTimeAttacked+enemy.attackCoolDown))
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        
        return false;
    }
}
