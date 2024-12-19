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
        if (Context.IsNearPlayer(Context.AttackRad))
        {
            Debug.Log("Attack!!!");
        }
        else
        {
            Context.ChangeState(EnemyStateCtr.States.Move);
        }
        
    }

    protected override void Exit()
    {
        Debug.Log("Exit Battle");
    }
}
