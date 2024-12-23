using IceMilkTea.StateMachine;
using UnityEngine;

public class EnemyState_Battle : ImtStateMachine<EnemyStateCtr>.State
{
    // ���̃X�e�[�g��player���U���͈͓��ɂ�����������

    // �U���Ԋu�p�̕ϐ�
    float attackTimer = 0;

    protected override void Enter()
    {
        Debug.Log("Enter Battle");
    }

    protected override void Update()
    {
        Context.OncolliderStay();
        if (Context.PlayeIsInside)
        {
            Context.SearchForEnemies(Context.PlayerCollider);
            if (Context.IsAttack)
            {
                // �����ɍU������

                if (Context.NavMeshAgent.remainingDistance <= Context.NavMeshAgent.stoppingDistance)
                {
                    attackTimer += Time.deltaTime;
                    Context.EnemyAnimater.SetBool("isPlayerNear", true);
                    Debug.Log(attackTimer);
                    Context.EnemyTransform.LookAt(Context.Player.transform);

                    if (attackTimer >= Context.EnemyCharaData.statusList[Context.EnemyCharaDataIndex].attackTime)
                    {
                        Context.EnemyAnimater.SetTrigger("isAttack");
                        Context.EnemyAnimater.SetInteger("attackNumber", Random.Range(0, 6));
                        attackTimer = 0;
                    }
                }
            }
            else
            {
                Context.EnemyAnimater.SetBool("isPlayerNear", false);
                Context.ChangeState(EnemyStateCtr.States.Move);
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
