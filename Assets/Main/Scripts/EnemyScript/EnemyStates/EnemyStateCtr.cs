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
    protected GameObject player = null;
    public GameObject Player => player;

    // �����ʒu
    protected Vector3 rootPos = new Vector3();
    public Vector3 RootPos => rootPos;

    // NearPlayer�p
    [SerializeField]
    protected ChaseAgent chaseAgent;
    public ChaseAgent ChaseAgent => chaseAgent;
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

        // �X�e�[�g�}�V���Z�b�g�A�b�v
        enemyStateMachine = new ImtStateMachine<EnemyStateCtr>(this);
        enemyStateMachine.AddTransition<EnemyState_Idle, EnemyState_Battle>((int)States.Battle);
        enemyStateMachine.AddTransition<EnemyState_Battle, EnemyState_Idle>((int)States.Idle);

        // �N���X�e�[�g��ݒ�
        enemyStateMachine.SetStartState<EnemyState_Idle>();
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
