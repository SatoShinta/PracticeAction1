using IceMilkTea.StateMachine;
using UnityEngine;

public class EnemyState_Battle : ImtStateMachine<EnemyStateCtr>.State
{
    protected override void Enter()
    {
        Debug.Log("Enter Battle");
    }

    protected override void Update()
    {
        Context.OncolliderStay();
        if (Context.PlayeIsInside)
        {
            Context.SearchForEnemies(Context.PlayerCollider);
            if (Context.IsAttack)
            {
                // ‚±‚±‚ÉUŒ‚ˆ—
                //Debug.Log("UŒ‚I");
                if (Context.NavMeshAgent.remainingDistance <= Context.NavMeshAgent.stoppingDistance)
                {
                    Context.EnemyAnimater.SetBool("isFindPlayer", false);
                    Context.EnemyAnimater.SetTrigger("isAttack");
                    Context.EnemyAnimater.SetInteger("attackNumber", Random.Range(0, 6));
                }
                //Context.AttackControler();
            }
            else
            {
                Context.ChangeState(EnemyStateCtr.States.Move);
            }
        }
        else
        {
            Context.ChangeState(EnemyStateCtr.States.Idle);
        }

    }

    protected override void Exit()
    {
        Debug.Log("Exit Battle");
    }
}
