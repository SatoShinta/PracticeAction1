using UnityEngine;

public class ColliderGet : MonoBehaviour
{
    [SerializeField] GameObject player = null;
    [SerializeField] PlayerAttackController attackController = null;


    private void Start()
    {
        attackController = player.GetComponent<PlayerAttackController>();
    }


    // 自分の目の前にいるエネミーのコライダーを取得する
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            attackController.EnemyCollider = other;
        }
    }
}
