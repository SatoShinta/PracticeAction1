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

    // ステートマシン
    private ImtStateMachine<EnemyStateCtr> enemyStateMachine;
    // 現在のステート
    protected States nowState = new States();

    // プレイヤー
    [SerializeField]
    protected GameObject player = null;
    public GameObject Player => player;

    // 初期位置
    [SerializeField]
    protected Vector3 rootPos = new Vector3();
    public Vector3 RootPos => rootPos;

    // NearPlayer用
    [SerializeField]
    protected float rad = 0;
    public float Rad => rad;
    [SerializeField]
    protected NavMeshAgent agent;
    public NavMeshAgent NavMeshAgent => agent;

    // 攻撃範囲
    [SerializeField]
    protected float attackRad = 0;
    public float AttackRad => attackRad;

    protected bool isEndMove = false;
    public bool IsEndMove => isEndMove;

    // レイの発射位置
    [SerializeField]
    protected Vector3 rayPosition;
    public Vector3 RayPosition => rayPosition;


    private void Awake()
    {
        // 初期位置保存
        rootPos = this.transform.position;

        // プレイヤーを取得する
        player = FindAnyObjectByType<PlayerController>()?.gameObject;

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
    /// 初期位置に戻るメソッド
    /// </summary>
    /// <param name="pos">初期位置</param>
    public void MoveRootPos(Vector3 pos)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, pos, NavMeshAgent.speed * Time.deltaTime);
        if (transform.position == pos)
        {
            isEndMove = true;
        }
        else
        {
            isEndMove = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Rayを飛ばす方向を計算
            Vector3 temp = other.transform.position - transform.position;
            Vector3 direction = temp.normalized;

            // Rayを飛ばす
            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(this.transform.position + rayPosition, ray.direction, Color.red);

            // 情報を保管
            RaycastHit hit;

            //最初に当たったオブジェクトを調べる
            if (Physics.Raycast(this.transform.position + rayPosition, ray.direction, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    player = other.gameObject;
                    agent.destination = player.transform.position;
                    Debug.Log("みつけた" + player.transform.position);
                    if (Physics.CheckSphere(transform.position, attackRad, LayerMask.GetMask("Player")))
                    {
                        // ここに攻撃処理
                        Debug.Log("攻撃開始！");
                    }
                }
            }
            else
            {
                Debug.Log("壁がある");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rad);
        Gizmos.DrawSphere(transform.position, attackRad);
    }

}
