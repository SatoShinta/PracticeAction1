using IceMilkTea.StateMachine;
using UnityEngine;

public class EnemyState_Idle : ImtStateMachine<EnemyStateCtr>.State
{

    /// <summary>
    ///  �X�e�[�g�J�n����
    /// </summary>
    protected override void Enter()
    {
        Debug.Log("Enter Idle");
    }

    protected override void Update()
    {
        // ���G
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
