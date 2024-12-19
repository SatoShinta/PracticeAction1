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
        if (!Context.IsNearPlayer(Context.Rad))  // プレイヤーが範囲外に出た場合
        {
            Context.NavMeshAgent.isStopped = true;
            Context.MoveRootPos(Context.RootPos);
            Context.ApproachTarget(Context.RootPos);

            // 目的地に到達したら Idle ステートに切り替え
            if (Context.IsEndMove)
            {
                Debug.Log("Switching to Idle state: Reached RootPos");
                Context.ChangeState(EnemyStateCtr.States.Idle);
            }
        }
        else // プレイヤーが範囲内に入った場合
        {
            Context.ApproachTarget(Context.Player.transform.position);
            if (Context.IsNearPlayer(Context.AttackRad))
            {
                Debug.Log("Switching to Battle state: Player in attack range");
                Context.ChangeState(EnemyStateCtr.States.Battle);
            }
        }

    }

    protected override void Exit()
    {
        Debug.Log("Exit Move");
    }

}
