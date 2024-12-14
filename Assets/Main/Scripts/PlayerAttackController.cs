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
    [SerializeField, Header("�v���C���[�̃q�b�g�X�g�b�v����")] float playerHitStopTime = 0.2f;
    public float PlayerHitStopTime
    {
        get { return playerHitStopTime; }
    }

    [SerializeField, Header("�v���C���[�̍U���Ԋu")] float attackTime = 0.2f;
    [SerializeField] List<Collider> attackColliders = new List<Collider>();
    [SerializeField] SerializableDictionary<string, int> attackColliderDictionary = null;
    [field: SerializeField]
    public Collider EnemyCollider { get; set; }
    float timer = 0; // �o�ߎ���



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
        Debug.Log(clipInfo[0].clip.name);
        // �U�����͈ړ��ł��Ȃ������i��]�͂ł���j
        if (clipInfo[0].clip.name.Contains("Place"))
        {
            playerController.Velocity = Vector3.zero;
        }
        HitStop();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        playerAnim.SetTrigger("isAttack");
        // �p���`
        playerAnim.SetInteger("attackType", 0);
    }

    void OnAttack2(InputAction.CallbackContext context)
    {
        playerAnim.SetTrigger("isAttack");
        // �L�b�N
        playerAnim.SetInteger("attackType", 1);
        timer = 0;

    }

    /// <summary>
    /// ���݂̃A�j���[�V�����̖��O���Q�Ƃ��āA���ꂼ��̓����蔻����o�����\�b�h
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
    /// �����蔻����폜���郁�\�b�h
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
    /// �q�b�g�X�g�b�v���������邽�߂̃��\�b�h
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
