using UnityEngine;

public class ColliderGet : MonoBehaviour
{
    [SerializeField] GameObject player = null;
    [SerializeField] PlayerAttackController attackController = null;


    private void Start()
    {
        attackController = player.GetComponent<PlayerAttackController>();
    }


    // �����̖ڂ̑O�ɂ���G�l�~�[�̃R���C�_�[���擾����
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            attackController.EnemyCollider = other;
        }
    }
}
