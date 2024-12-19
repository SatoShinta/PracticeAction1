using UnityEngine;
using IceMilkTea.StateMachine;
using Unity.Cinemachine;
using UnityEngine.AI;
using Unity.VisualScripting;
public class EnemyStateCtr : MonoBehaviour
{
    public enum States
    {
        Idle,
        Battle,
    }

    // �X�e�[�g�}�V��
    private ImtStateMachine<EnemyStateCtr> enemyStateMachine;
    // ���݂̃X�e�[�g
    protected States nowState = new States();

    // �v���C���[
    [SerializeField]
    protected GameObject player = null;
    public GameObject Player => player;

    // �����ʒu
    [SerializeField]
    protected Vector3 rootPos = new Vector3();
    public Vector3 RootPos => rootPos;

    // NearPlayer�p
    [SerializeField]
    protected float rad = 0;
    public float Rad => rad;
    [SerializeField]
    protected NavMeshAgent agent;
    public NavMeshAgent NavMeshAgent => agent;

    // �U���͈�
    [SerializeField] 
    protected float attackRad = 0;
    public float AttackRad => attackRad;


    private void Awake()
    {
        // �����ʒu�ۑ�
        rootPos = this.transform.position;

        // �v���C���[���擾����
        player = FindAnyObjectByType<PlayerController>()?.gameObject;

        // �X�e�[�g�}�V���Z�b�g�A�b�v
        enemyStateMachine = new ImtStateMachine<EnemyStateCtr>(this);
        enemyStateMachine.AddTransition<EnemyState_Idle, EnemyState_Battle>((int)States.Battle);
        enemyStateMachine.AddTransition<EnemyState_Battle, EnemyState_Idle>((int)States.Idle);

        // �N���X�e�[�g��ݒ�
        enemyStateMachine.SetStartState<EnemyState_Idle>();

        // NavMeshAgent���N�����Ă���
        NavMeshAgent.enabled = true;
    }

    private void Update()
    {
        enemyStateMachine.Update();
    }

    /// <summary>
    /// �X�e�[�g�̕ύX����
    /// </summary>
    /// <param name="nextStates">���̃X�e�[�g</param>
    public void ChangeState(States nextStates)
    {
        enemyStateMachine.SendEvent((int)nextStates);
        nowState = nextStates;
    }

    /// <summary>
    /// player���͈͓��ɓ��������m�F���郁�\�b�h
    /// </summary>
    /// <param name="rad">���G�͈�</param>
    /// <returns></returns>
    public bool IsNearPlayer(float rad)
    {
        if (Physics.CheckSphere(transform.position, rad, LayerMask.GetMask("Player")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// player�ɋ߂Â����\�b�h
    /// </summary>
    /// <param name="target">�ړI�n</param>
    public void ApproachTarget(Vector3 target)
    {
        agent.destination = target;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rad);
        Gizmos.DrawSphere(transform.position, attackRad);
    }

}
