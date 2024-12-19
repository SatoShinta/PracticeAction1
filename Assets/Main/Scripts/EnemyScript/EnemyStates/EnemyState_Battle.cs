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
            Context.ApproachTarget(Context.Player.transform.position);

            if (Context.IsNearPlayer(Context.AttackRad) && Context.IsNearPlayer(Context.Rad))
            {
                // Ç±Ç±Ç…çUåÇèàóù
                Debug.Log("attack!");
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
