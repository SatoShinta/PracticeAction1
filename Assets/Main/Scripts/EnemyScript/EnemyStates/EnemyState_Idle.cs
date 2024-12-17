using IceMilkTea.StateMachine;
using System.Linq.Expressions;
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
