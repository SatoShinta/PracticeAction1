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
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rad);
        Gizmos.DrawSphere(transform.position, attackRad);
    }

}
