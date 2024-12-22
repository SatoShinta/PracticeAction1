using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.AI;
public class EnemyStateCtr : MonoBehaviour
{
    public enum States
    {
        Idle,
        Battle,
        Move,
    }

    // ���̃I�u�W�F�N�g��transform
    protected Transform enemyTransform = default;
    public Transform EnemyTransform => enemyTransform;

    // �X�e�[�g�}�V��
    private ImtStateMachine<EnemyStateCtr> enemyStateMachine;
    // ���݂̃X�e�[�g
    protected States nowState = new States();

    // �v���C���[
    [SerializeField] protected GameObject player = null;
    public GameObject Player => player;
    protected Collider playerCollider = null;
    public Collider PlayerCollider => playerCollider;

    // ���G�p�R���C�_�[
    [SerializeField] protected Collider searchCollider = null;
    public Collider SearchCollider => searchCollider;

    // �i�r���b�V���G�[�W�F���g
    [SerializeField] protected NavMeshAgent agent;
    public NavMeshAgent NavMeshAgent => agent;

    // Animator�擾�p
    [SerializeField] protected Animator enemyAnimator;
    public Animator EnemyAnimater => enemyAnimator;

    // �L�����f�[�^�擾�p
    [SerializeField] protected CharaData enemyCharaData;
    public CharaData EnemyCharaData => enemyCharaData;
    // �L�����f�[�^�̃C���f�b�N�X�ԍ�
    [SerializeField] protected int enemyCharaDataIndex;
    public int EnemyCharaDataIndex => enemyCharaDataIndex;

    // �U���͈�
    [SerializeField] protected float attackRad = 0;
    public float AttackRad => attackRad;

    // ���C�̔��ˈʒu
    [SerializeField] protected Vector3 rayPosition;
    public Vector3 RayPosition => rayPosition;

    // �����ʒu
    [SerializeField] protected Vector3 rootPos = new Vector3();
    public Vector3 RootPos => rootPos;

    // �t���O�B
    protected bool isAttack = false;
    public bool IsAttack => isAttack;

    protected bool playeIsInside = false;
    public bool PlayeIsInside => playeIsInside;


    private void Awake()
    {
        // �����ʒu�ۑ�
        rootPos = this.transform.position;

        // �v���C���[���擾����
        player = FindAnyObjectByType<PlayerController>()?.gameObject;
        playerCollider = player.GetComponent<Collider>();

        // �X�e�[�g�}�V���Z�b�g�A�b�v
        enemyStateMachine = new ImtStateMachine<EnemyStateCtr>(this);
        enemyStateMachine.AddTransition<EnemyState_Idle, EnemyState_Battle>((int)States.Battle);
        enemyStateMachine.AddTransition<EnemyState_Battle, EnemyState_Idle>((int)States.Idle);
        enemyStateMachine.AddTransition<EnemyState_Idle, EnemyState_Move>((int)States.Move);
        enemyStateMachine.AddTransition<EnemyState_Battle, EnemyState_Move>((int)States.Move);
        enemyStateMachine.AddTransition<EnemyState_Move, EnemyState_Idle>((int)States.Idle);
        enemyStateMachine.AddTransition<EnemyState_Move, EnemyState_Battle>((int)States.Battle);

        // �N���X�e�[�g��ݒ�
        enemyStateMachine.SetStartState<EnemyState_Idle>();

        // NavMeshAgent���N�����Ă���
        NavMeshAgent.enabled = true;
    }

    private void Update()
    {
        enemyStateMachine.Update();
        enemyTransform = this.gameObject.transform;
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
        //Debug.Log(target.ToString());
    }

    ///// <summary>
    ///// �����ʒu�ɖ߂郁�\�b�h
    ///// </summary>
    ///// <param name="pos">�����ʒu</param>
    //public void MoveRootPos(Vector3 pos)
    //{
    //    this.transform.position = Vector3.MoveTowards(this.transform.position, pos, NavMeshAgent.speed * Time.deltaTime);
    //    if (transform.position == pos)
    //    {
    //        isEndMove = true;
    //    }
    //    else
    //    {
    //        isEndMove = false;
    //    }
    //}

    public void OncolliderStay()
    {
        // OverlapBox���g�p���āA���̃I�u�W�F�N�g�ɐݒ肳��Ă�����G�p�R���C�_�[�͈͓̔��ɑ��݂���A"Player"���C���[�����������ׂẴR���C�_�[��colliders�z��Ɋi�[����
        Collider[] colliders = Physics.OverlapBox(searchCollider.bounds.center, searchCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Player"));
        playeIsInside = false; // �v���C���[�����ɍ��G�͈͓��ɂ��邩�ǂ����̃t���O

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                playeIsInside = true;
            }

        }
        if (!playeIsInside)
        {
            // Debug.Log("�A��܂�");
        }

    }


    public void SearchForEnemies(Collider col)
    {
        Vector3 direction = (col.transform.position - transform.position).normalized;

        Ray ray = new Ray(transform.position + rayPosition, direction);
        Debug.DrawRay(this.transform.position + rayPosition, ray.direction, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                player = hit.collider.gameObject;
                agent.destination = player.transform.position;
                //Debug.Log("������" + player.transform.position);

                if (Physics.CheckSphere(transform.position, attackRad, LayerMask.GetMask("Player")))
                {
                    // �����ɍU������
                    isAttack = true;
                }
                else
                {
                    isAttack = false;
                }
            }
            else
            {
                // Debug.Log("�ǂ�����");
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(transform.position, attackRad);
    }

}
