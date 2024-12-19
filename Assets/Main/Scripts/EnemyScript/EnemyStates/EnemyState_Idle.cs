using IceMilkTea.StateMachine;
using UnityEngine;

public class EnemyState_Idle : ImtStateMachine<EnemyStateCtr>.State
{

    /// <summary>
    ///  ステート開始処理
    /// </summary>
    protected override void Enter()
    {
        Debug.Log("Enter Idle");
    }

    protected override void Update()
    {
        // 索敵
        if (Context.IsNearPlayer(Context.Rad))
        {
            Context.ChangeState(EnemyStateCtr.States.Battle);
        }
        else
        {
            Context.ApproachTarget(Context.RootPos);
        }

    }

    protected override void Exit()
    {
        Debug.Log("Exit Idle");
    }

}
