using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField, Header("ダメージを受けた回数")] int damageCounter = 0;
    [SerializeField, Header("敵のHP")] int enemyHP = 0;
    [SerializeField] SkinnedMeshRenderer enemySkinnedMeshRenderer;
    [SerializeField] Collider enemyCollider;
    [SerializeField] CharaData charaData;
    [SerializeField] Slider hpSlider;
    [SerializeField] Canvas hpCanvas;
    [SerializeField, Header("ステータスデータの番号")] int index = 0;
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
    /// ダメージを受けたときの処理
    /// </summary>
    public void DamageReaction()
    {
        // 攻撃を受けている間はアニメーションをストップする処理
        var seq = DOTween.Sequence();
        // 画面の振動演出
        seq.Append(transform.DOShakePosition(pAttackController.PlayerHitStopTime, 0.15f, 25, fadeOut: false));
        //enemyAnim.speed = 0f;
        //seq.SetDelay(pAttackController.PlayerHitStopTime);
        seq.AppendCallback(() => enemyAnim.speed = 1f);

        damageCounter++;
        enemyAnim.SetTrigger("isHit");
        enemyAnim.SetInteger("hitNumber", Random.Range(0, 4));

        // 敵のHPの計算を行う
        enemyHP = enemyHP - charaData.statusList[playerIndex].atk;

        hpSlider.value = enemyHP;
    }

    /// <summary>
    /// スライダーの初期値を設定するメソッド
    /// </summary>
    public void SetHPValue()
    {
        hpSlider.maxValue = enemyHP;
        hpSlider.value = enemyHP;
    }

    /// <summary>
    /// 死んだときの処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
