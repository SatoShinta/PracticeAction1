using System.Collections;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField, Header("�_���[�W���󂯂���")] int damageCounter = 0;
    [SerializeField] SkinnedMeshRenderer enemySkinnedMeshRenderer;
    [SerializeField] Collider enemyCollider;


    private void Start()
    {
        enemySkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyCollider = GetComponent<Collider>();  
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
        }
    }

    public IEnumerator EnemyDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
