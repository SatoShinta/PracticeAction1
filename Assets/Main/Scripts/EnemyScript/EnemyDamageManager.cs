using System.Collections;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField, Header("ƒ_ƒ[ƒW‚ğó‚¯‚½‰ñ”")] int damageCounter = 0;
    [SerializeField] SkinnedMeshRenderer enemySkinnedMeshRenderer;
    [SerializeField] Collider enemyCollider;
    Animator enemyAnim = null;


    private void Start()
    {
        enemySkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyCollider = GetComponent<Collider>();  
        enemyAnim = GetComponent<Animator>();
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
            enemyAnim.SetInteger("hitNumber",Random.Range(damageCounter, 3));
            enemyAnim.SetBool("isNomal",false);
        }
    }

    public IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
