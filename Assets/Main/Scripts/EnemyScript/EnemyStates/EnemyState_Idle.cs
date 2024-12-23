using IceMilkTea.StateMachine;
using UnityEngine;

public class EnemyState_Idle : ImtStateMachine<EnemyStateCtr>.State
{

    // このステートはエネミーが初期位置にいる状態を示す

    /// <summary>
    ///  ステート開始処理
    /// </summary>
    protected override void Enter()
    {
        Debug.Log("Enter Idle");
    }

    protected override void Update()
    {
        Context.EnemyAnimater.SetBool("isFindPlayer",false);
        Context.EnemyAnimater.SetBool("isMovingToStartPosition", false);
        Context.EnemyAnimater.SetBool("isInitialPosition",true);

        Context.OncolliderStay();
        if (Context.PlayeIsInside)
        {
            Context.EnemyAnimater.SetBool("isInitialPosition", false);
            Context.ChangeState(EnemyStateCtr.States.Move);
        }

    }

    protected override void Exit()
    {
        Debug.Log("Exit Idle");
    }

}
