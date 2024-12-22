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

    // このオブジェクトのtransform
    protected Transform enemyTransform = default;
    public Transform EnemyTransform => enemyTransform;

    // ステートマシン
    private ImtStateMachine<EnemyStateCtr> enemyStateMachine;
    // 現在のステート
    protected States nowState = new States();

    // プレイヤー
    [SerializeField] protected GameObject player = null;
    public GameObject Player => player;
    protected Collider playerCollider = null;
    public Collider PlayerCollider => playerCollider;

    // 索敵用コライダー
    [SerializeField] protected Collider searchCollider = null;
    public Collider SearchCollider => searchCollider;

    // ナビメッシュエージェント
    [SerializeField] protected NavMeshAgent agent;
    public NavMeshAgent NavMeshAgent => agent;

    // Animator取得用
    [SerializeField] protected Animator enemyAnimator;
    public Animator EnemyAnimater => enemyAnimator;

    // キャラデータ取得用
    [SerializeField] protected CharaData enemyCharaData;
    public CharaData EnemyCharaData => enemyCharaData;
    // キャラデータのインデックス番号
    [SerializeField] protected int enemyCharaDataIndex;
    public int EnemyCharaDataIndex => enemyCharaDataIndex;

    // 攻撃範囲
    [SerializeField] protected float attackRad = 0;
    public float AttackRad => attackRad;

    // レイの発射位置
    [SerializeField] protected Vector3 rayPosition;
    public Vector3 RayPosition => rayPosition;

    // 初期位置
    [SerializeField] protected Vector3 rootPos = new Vector3();
    public Vector3 RootPos => rootPos;

    // フラグ達
    protected bool isAttack = false;
    public bool IsAttack => isAttack;

    protected bool playeIsInside = false;
    public bool PlayeIsInside => playeIsInside;


    private void Awake()
    {
        // 初期位置保存
        rootPos = this.transform.position;

        // プレイヤーを取得する
        player = FindAnyObjectByType<PlayerController>()?.gameObject;
        playerCollider = player.GetComponent<Collider>();

        // ステートマシンセットアップ
        enemyStateMachine = new ImtStateMachine<EnemyStateCtr>(this);
        enemyStateMachine.AddTransition<EnemyState_Idle, EnemyState_Battle>((int)States.Battle);
        enemyStateMachine.AddTransition<EnemyState_Battle, EnemyState_Idle>((int)States.Idle);
        enemyStateMachine.AddTransition<EnemyState_Idle, EnemyState_Move>((int)States.Move);
        enemyStateMachine.AddTransition<EnemyState_Battle, EnemyState_Move>((int)States.Move);
        enemyStateMachine.AddTransition<EnemyState_Move, EnemyState_Idle>((int)States.Idle);
        enemyStateMachine.AddTransition<EnemyState_Move, EnemyState_Battle>((int)States.Battle);

        // 起動ステートを設定
        enemyStateMachine.SetStartState<EnemyState_Idle>();

        // NavMeshAgentを起動しておく
        NavMeshAgent.enabled = true;
    }

    private void Update()
    {
        enemyStateMachine.Update();
        enemyTransform = this.gameObject.transform;
    }

    /// <summary>
    /// ステートの変更処理
    /// </summary>
    /// <param name="nextStates">次のステート</param>
    public void ChangeState(States nextStates)
    {
        enemyStateMachine.SendEvent((int)nextStates);
        nowState = nextStates;
    }

    /// <summary>
    /// playerが範囲内に入ったか確認するメソッド
    /// </summary>
    /// <param name="rad">索敵範囲</param>
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
    /// playerに近づくメソッド
    /// </summary>
    /// <param name="target">目的地</param>
    public void ApproachTarget(Vector3 target)
    {
        agent.destination = target;
        //Debug.Log(target.ToString());
    }

    ///// <summary>
    ///// 初期位置に戻るメソッド
    ///// </summary>
    ///// <param name="pos">初期位置</param>
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
        // OverlapBoxを使用して、このオブジェクトに設定されている索敵用コライダーの範囲内に存在する、"Player"レイヤーを持ったすべてのコライダーをcolliders配列に格納する
        Collider[] colliders = Physics.OverlapBox(searchCollider.bounds.center, searchCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Player"));
        playeIsInside = false; // プレイヤーが中に索敵範囲内にいるかどうかのフラグ

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                playeIsInside = true;
            }

        }
        if (!playeIsInside)
        {
            // Debug.Log("帰ります");
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
                //Debug.Log("見つけた" + player.transform.position);

                if (Physics.CheckSphere(transform.position, attackRad, LayerMask.GetMask("Player")))
                {
                    // ここに攻撃処理
                    isAttack = true;
                }
                else
                {
                    isAttack = false;
                }
            }
            else
            {
                // Debug.Log("壁がある");
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(transform.position, attackRad);
    }

}
