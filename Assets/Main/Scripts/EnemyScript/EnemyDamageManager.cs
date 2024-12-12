using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField] PlayerAttackController pAttackController;
    [SerializeField, Header("É_ÉÅÅ[ÉWÇéÛÇØÇΩâÒêî")] int damageCounter = 0;
    [SerializeField] SkinnedMeshRenderer enemySkinnedMeshRenderer;
    [SerializeField] Collider enemyCollider;
    Animator enemyAnim = null;
    GameObject player = null;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyCollider = GetComponent<Collider>();  
        enemyAnim = GetComponent<Animator>();
        pAttackController =player.GetComponent<PlayerAttackController>();
    }

    private void Update()
    {
        if (damageCounter > 4)
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
            damageCounter++;
            enemyAnim.SetTrigger("isHit");
            enemyAnim.SetInteger("hitNumber",Random.Range(0, 4));
            enemyAnim.SetBool("isNomal",false);
        }
    }

    public IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
