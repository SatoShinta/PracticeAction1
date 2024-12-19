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
        if (!Context.IsNearPlayer(Context.Rad))  // �v���C���[���͈͊O�ɏo���ꍇ
        {
            Context.NavMeshAgent.isStopped = true;
            Context.MoveRootPos(Context.RootPos);
            Context.ApproachTarget(Context.RootPos);

            // �ړI�n�ɓ��B������ Idle �X�e�[�g�ɐ؂�ւ�
            if (Context.IsEndMove)
            {
                Debug.Log("Switching to Idle state: Reached RootPos");
                Context.ChangeState(EnemyStateCtr.States.Idle);
            }
        }
        else // �v���C���[���͈͓��ɓ������ꍇ
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
