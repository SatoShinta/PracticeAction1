using IceMilkTea.StateMachine;
using UnityEngine;

public class EnemyState_Move : ImtStateMachine<EnemyStateCtr>.State
{
    protected override void Enter()
    {
        Debug.Log("Enter Move");
    }

    protected override void Update()
    {
        Context.OncolliderStay();
        if (Context.PlayeIsInside)
        {
            Context.EnemyAnimater.SetBool("isFindPlayer", true);
            Context.SearchForEnemies(Context.PlayerCollider);
            if (Context.IsAttack)
            {
                Context.ChangeState(EnemyStateCtr.States.Battle);
            }
        }
        else
        {
            Context.EnemyAnimater.SetBool("isMovingToStartPosition",true);
            Context.EnemyAnimater.SetBool("isFindPlayer", false);

            Context.NavMeshAgent.destination = Context.RootPos;
            if (Context.NavMeshAgent.remainingDistance <= Context.NavMeshAgent.stoppingDistance)
            {
                Context.ChangeState(EnemyStateCtr.States.Idle);
            }
        }

    }

    protected override void Exit()
    {
        Debug.Log("Exit Move");
    }

}
