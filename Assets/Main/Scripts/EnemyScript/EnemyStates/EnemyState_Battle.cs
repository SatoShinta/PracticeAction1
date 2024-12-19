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
        if (Context.IsNearPlayer(Context.Rad))
        {
            if (Context.IsNearPlayer(Context.AttackRad))
            {
                // Ç±Ç±Ç…çUåÇèàóù
                Debug.Log("attack!");
            }
            else
            {
                Context.ApproachTarget(Context.Player.transform.position);
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
