using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    InputSystem_Actions inputAction;
    Animator playerAnim;
    Rigidbody playerRigit;
    PlayerController playerController;
    AnimatorClipInfo[] clipInfo;

    [SerializeField] List<Collider> attackColliders = new List<Collider>();
    [SerializeField] SerializableDictionary<string, int> attackColliderDictionary = null;

    // bool isAttack = false;

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

        //switch (clipInfo[0].clip.name)
        //{
        //    case "H2H_JabInPlace":
        //        attackCollider[0].enabled = true;
        //        break;
        //    case "H2H_StraightPunchInPlace":
        //        attackCollider[1].enabled = true;
        //        break;
        //    case "H2H_HookPunch_InPlace":
        //        attackCollider[2].enabled = true;
        //        break;
        //    case "H2H_SpinningHookKick_InPlace":
        //        attackCollider[3].enabled = true;
        //        break;
        //}
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

        //switch (clipInfo[0].clip.name)
        //{
        //    case "H2H_JabInPlace":
        //        attackCollider[0].enabled = false;
        //        break;
        //    case "H2H_StraightPunchInPlace":
        //        attackCollider[1].enabled = false;
        //        break;
        //    case "H2H_HookPunch_InPlace":
        //        attackCollider[2].enabled = false;
        //        break;
        //    case "H2H_SpinningHookKick_InPlace":
        //        attackCollider[3].enabled = false;
        //        break;
        //}
    }
    private void OnDestroy()
    {
        inputAction?.Dispose();
    }
}
