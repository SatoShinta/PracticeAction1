using IceMilkTea.StateMachine;
using System.Linq.Expressions;
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
        if (Context.NearPlayer())
        {
            Context.ChangeState(EnemyStateCtr.States.Battle);
        }
    }

    protected  override void Exit()
    {
        Debug.Log("Exit Idle");
    }

}
