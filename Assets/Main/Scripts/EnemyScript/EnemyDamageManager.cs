using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField, Header("�_���[�W���󂯂���")] int damageCounter = 0;
    [SerializeField,Header("�G��HP")] int enemyHP = 0;
    [SerializeField] SkinnedMeshRenderer enemySkinnedMeshRenderer;
    [SerializeField] Collider enemyCollider;
    [SerializeField] public bool isDamage = false;
    Animator enemyAnim = null;
    GameObject player = null;
    PlayerAttackController pAttackController = null;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyCollider = GetComponent<Collider>();
        enemyAnim = GetComponent<Animator>();
        pAttackController = player.GetComponent<PlayerAttackController>();
    }

    private void Update()
    {
        if (damageCounter > enemyHP)
        {
            enemySkinnedMeshRenderer.enabled = false;
            enemyCollider.enabled = false;
            StartCoroutine(EnemyDestroy());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttackCollider")
        {
            isDamage = true;
            // �U�����󂯂Ă���Ԃ̓A�j���[�V�������X�g�b�v���鏈��
            var seq = DOTween.Sequence();
            seq.AppendCallback(() => enemyAnim.speed = 0f);
            seq.SetDelay(pAttackController.PlayerHitStopTime);
            seq.AppendCallback(() => enemyAnim.speed = 1f);

            damageCounter++;
            enemyAnim.SetTrigger("isHit");
            enemyAnim.SetInteger("hitNumber", Random.Range(0, 4));
            enemyAnim.SetBool("isNomal", false);
        }
        else
        {
            isDamage = false;
        }
    }

    public IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
