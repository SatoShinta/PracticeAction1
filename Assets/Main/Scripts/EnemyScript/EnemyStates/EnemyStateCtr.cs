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


    /// <summary>
    /// OntrrigerStayと同じ処理を行うためにつくったメソッド
    /// </summary>
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


    /// <summary>
    /// playerを索敵するメソッド
    /// </summary>
    /// <param name="col">playerのコライダー</param>
    public void SearchForEnemies(Collider col)
    {
        // playerの方向を取得
        Vector3 direction = (col.transform.position - transform.position).normalized;

        // レイを飛ばしてplayerの情報を取得
        Ray ray = new Ray(transform.position + rayPosition, direction);
        Debug.DrawRay(this.transform.position + rayPosition, ray.direction, Color.red);
        RaycastHit hit;

        // レイが当たったら以下の処理
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                player = hit.collider.gameObject;
                // ナビメッシュエージェントの目的地をplayerに設定
                agent.destination = player.transform.position;

                // 攻撃範囲内（attackRad）にplayerが入ったら以下の処理
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
