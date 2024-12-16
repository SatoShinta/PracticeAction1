using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField, Header("�_���[�W���󂯂���")] int damageCounter = 0;
    [SerializeField, Header("�G��HP")] int enemyHP = 0;
    [SerializeField] SkinnedMeshRenderer enemySkinnedMeshRenderer;
    [SerializeField] Collider enemyCollider;
    [SerializeField] CharaData charaData;
    [SerializeField] Slider hpSlider;
    [SerializeField] Canvas hpCanvas;
    [SerializeField, Header("�X�e�[�^�X�f�[�^�̔ԍ�")] int index = 0;
    Animator enemyAnim = null;
    GameObject player = null;
    PlayerAttackController pAttackController = null;
    int playerIndex = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyCollider = GetComponent<Collider>();
        enemyAnim = GetComponent<Animator>();
        pAttackController = player.GetComponent<PlayerAttackController>();
        enemyHP = charaData.statusList[index].maxHp;
        SetHPValue();
        playerIndex = charaData.playerIndex;
    }

    private void Update()
    {
        if (enemyHP <= 0)
        {
            enemySkinnedMeshRenderer.enabled = false;
            enemyCollider.enabled = false;
            hpCanvas.enabled = false;
            StartCoroutine(EnemyDestroy());
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAttackCollider"))
        {
            DamageReaction();
        }
    }



    /// <summary>
    /// �_���[�W���󂯂��Ƃ��̏���
    /// </summary>
    public void DamageReaction()
    {
        // �U�����󂯂Ă���Ԃ̓A�j���[�V�������X�g�b�v���鏈��
        var seq = DOTween.Sequence();
        // ��ʂ̐U�����o
        seq.Append(transform.DOShakePosition(pAttackController.PlayerHitStopTime, 0.15f, 25, fadeOut: false));
        //enemyAnim.speed = 0f;
        //seq.SetDelay(pAttackController.PlayerHitStopTime);
        seq.AppendCallback(() => enemyAnim.speed = 1f);

        damageCounter++;
        enemyAnim.SetTrigger("isHit");
        enemyAnim.SetInteger("hitNumber", Random.Range(0, 4));

        // �G��HP�̌v�Z���s��
        enemyHP = enemyHP - charaData.statusList[playerIndex].atk;

        hpSlider.value = enemyHP;
    }

    /// <summary>
    /// �X���C�_�[�̏����l��ݒ肷�郁�\�b�h
    /// </summary>
    public void SetHPValue()
    {
        hpSlider.maxValue = enemyHP;
        hpSlider.value = enemyHP;
    }

    /// <summary>
    /// ���񂾂Ƃ��̏���
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
