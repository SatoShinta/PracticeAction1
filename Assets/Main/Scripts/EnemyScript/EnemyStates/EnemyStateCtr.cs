using UnityEngine;
using IceMilkTea.StateMachine;
using Unity.Cinemachine;
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

    // プレイヤー制御クラス
    protected PlayerController player = null;
    public PlayerController PlayerController => player;

    // 初期位置
    protected Vector3 rootPos = new Vector3();
    public Vector3 RootPos => rootPos;

    private void Awake()
    {
        // 初期位置保存
        rootPos = this.transform.position;

        // プレイヤー
        player = FindAnyObjectByType<PlayerController>();

        // ステートマシンセットアップ
        enemyStateMachine = new ImtStateMachine<EnemyStateCtr>(this);
        enemyStateMachine.AddTransition<EnemyState_Idle, EnemyState_Battle>((int)States.Battle);
        enemyStateMachine.AddTransition<EnemyState_Battle, EnemyState_Idle>((int)States.Idle);

        // 起動ステートを設定
        enemyStateMachine.SetStartState<EnemyState_Idle>();
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

    public bool NearPlayer()
    {
        if (Physics.CheckSphere(transform.position, 7, LayerMask.GetMask("Player")))
        {
           return true;
        }
        else
        {
           return false;
        }
    }
   

}
