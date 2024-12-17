using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AttackAnimatorState
{
    H2H_JabInPlace,
    H2H_StraightPunchInPlace,
    H2H_HookPunch_InPlace,
    H2H_SpinningHookKick_InPlace
}

public class PlayerAttackController : MonoBehaviour
{
    InputSystem_Actions inputAction;
    Animator playerAnim;
    Rigidbody playerRigit;
    PlayerController playerController;
    AnimatorClipInfo[] clipInfo;
    [SerializeField, Header("プレイヤーのヒットストップ時間")] float playerHitStopTime = 0.2f;
    public float PlayerHitStopTime
    {
        get { return playerHitStopTime; }
    }

    [SerializeField] List<Collider> attackColliders = new List<Collider>();
    [SerializeField] SerializableDictionary<string, int> attackColliderDictionary = null;
    [field: SerializeField]
    public Collider EnemyCollider { get; set; }

    // ↓Enum型で検索をした場合に必要なやつ
    //[SerializeField]
    //private SerializableDictionary<AttackAnimatorState, int> keyValuePairs = default;

    //private readonly SerializableDictionary<AttackAnimatorState, string> pairs = new()
    //{
    //    { AttackAnimatorState.None, "" },
    //};



    void Start()
    {
        playerAnim = GetComponent<Animator>();
        inputAction = new InputSystem_Actions();
        playerRigit = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();

        inputAction.Player.Attack.started += OnAttack;
        inputAction.Player.Attack2.started += OnAttack2;
        inputAction.Enable();
    }

    void Update()
    {
        clipInfo = playerAnim.GetCurrentAnimatorClipInfo(0);
        //Debug.Log(clipInfo[0].clip.name);
        // 攻撃中は移動できなくした（回転はできる）
        if (clipInfo[0].clip.name.Contains("Place"))
        {
            playerController.Velocity = Vector3.zero;
        }
        HitStop();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        playerAnim.SetTrigger("isAttack");
        // パンチ
        playerAnim.SetInteger("attackType", 0);
    }

    void OnAttack2(InputAction.CallbackContext context)
    {
        playerAnim.SetTrigger("isAttack");
        // キック
        playerAnim.SetInteger("attackType", 1);
    }

    /// <summary>
    /// 現在のアニメーションの名前を参照して、それぞれの当たり判定を出すメソッド
    /// </summary>
    public void ColliderSet()
    {
        string animName = clipInfo[0].clip.name;

        if (attackColliderDictionary.ContainsKey(animName))
        {
            attackColliders[attackColliderDictionary[animName]].enabled = true;
        }
    }

    ///// <summary>
    ///// 現在のアニメーションの名前を参照して、それぞれの当たり判定を出すメソッド
    ///// Enum版！！！
    ///// </summary>
    //public void ColliderSet()
    //{
    //    string animName = clipInfo[0].clip.name;

    //    AttackAnimatorState attackState;
    //    if(Enum.TryParse(animName,out attackState)) // 文字列からEnum型に変換
    //    {
    //        if(keyValuePairs.ContainsKey(attackState))
    //        {
    //            attackColliders[keyValuePairs[attackState]].enabled = true;
    //        }
    //    }
    //}

    /// <summary>
    /// 当たり判定を削除するメソッド
    /// </summary>
    public void ColliderRemove()
    {
        string animName = clipInfo[0].clip.name;

        if (attackColliderDictionary.ContainsKey(animName))
        {
            attackColliders[attackColliderDictionary[animName]].enabled = false;
        }
    }


    /// <summary>
    /// ヒットストップを実装するためのメソッド
    /// </summary>
    public void HitStop()
    {
        if (EnemyCollider != null)
        {
            foreach (var collider in attackColliders)
            {
                if (collider != null && collider.bounds.Intersects(EnemyCollider.bounds))
                {
                    playerAnim.speed = 0f;
                    var seq = DOTween.Sequence();
                    seq.SetDelay(playerHitStopTime);
                    seq.AppendCallback(() => playerAnim.speed = 1f);
                }
                else
                {
                    collider.enabled = false;
                }
            }
        }
    }


    private void OnDestroy()
    {
        inputAction?.Dispose();
    }

}
