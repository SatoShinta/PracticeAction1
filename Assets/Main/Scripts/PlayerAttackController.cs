using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    InputSystem_Actions inputAction;
    public Animator playerAnim;
    Rigidbody playerRigit;
    PlayerController playerController;
    AnimatorClipInfo[] clipInfo;
    [SerializeField, Header("プレイヤーのヒットストップ時間")] float playerHitStopTime = 0.2f;

    public float PlayerHitStopTime
    {
        get { return playerHitStopTime; }
    }

    [SerializeField, Header("プレイヤーの攻撃間隔")] float attackTime = 0.2f;
    [SerializeField, Header("プレイヤーが攻撃できるかどうか")] bool isAttackOk = false;
    [SerializeField] List<Collider> attackColliders = new List<Collider>();
    [SerializeField] SerializableDictionary<string, int> attackColliderDictionary = null;
    [field: SerializeField]
    public Collider EnemyCollider { get; set; }
    float timer = 0; // 経過時間



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
        timer += Time.deltaTime;
        AttackTimer();

        clipInfo = playerAnim.GetCurrentAnimatorClipInfo(0);
        Debug.Log(clipInfo[0].clip.name);
        // 攻撃中は移動できなくした（回転はできる）
        if (clipInfo[0].clip.name.Contains("Place"))
        {
            playerController.Velocity = Vector3.zero;
        }

        ColliderDefaultState();
        HitStop();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (isAttackOk)
        {
            playerAnim.SetTrigger("isAttack");
            // パンチ
            playerAnim.SetInteger("attackType", 0);
            timer = 0;
        }
    }

    void OnAttack2(InputAction.CallbackContext context)
    {
        if (isAttackOk)
        {
            playerAnim.SetTrigger("isAttack");
            // キック
            playerAnim.SetInteger("attackType", 1);
            timer = 0;
        }

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

    public void ColliderDefaultState()
    {
        if(!isAttackOk)
        {
            foreach (var colider in attackColliders)
            {
                colider.enabled = false;
            }
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

    /// <summary>
    /// 攻撃間隔をあけるメソッド
    /// </summary>
    public void AttackTimer()
    {
        if (timer >= attackTime)
        {
            isAttackOk = true;
        }
        else
        {
            isAttackOk = false;
        }

    }

    private void OnDestroy()
    {
        inputAction?.Dispose();
    }

}
