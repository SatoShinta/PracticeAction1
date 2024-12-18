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
        if (Context.IsNearPlayer(Context.Rad))
        {
            Context.NavMeshAgent.enabled = true;
            Context.ApproachTarget(Context.Player.transform.position);
            // Ç±Ç±Ç…çUåÇèàóù
            if (Context.IsNearPlayer(Context.AttackRad))
            {

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
