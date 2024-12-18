using IceMilkTea.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyState_Battle : ImtStateMachine<EnemyStateCtr>.State
{
    protected override void Enter()
    {
        Debug.Log("Enter Battle");
    }

    protected override void Update()
    {
        if (Context.NearPlayer())
        {
            Context.NavMeshAgent.enabled = true;
            // ‚±‚±‚ÉUŒ‚ˆ—
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
