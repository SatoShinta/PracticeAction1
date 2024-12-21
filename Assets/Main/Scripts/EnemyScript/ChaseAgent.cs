using UnityEngine;
using UnityEngine.AI;

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float attackRad = 0f;
    [SerializeField] Vector3 rootPos = Vector3.zero; // �����ʒu
    [SerializeField] Vector3 rayPosition = Vector3.zero;
    NavMeshAgent agent;
    float distance = 10f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        rootPos = transform.position;
    }

    void Update()
    {
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Ray���΂��������v�Z
            Vector3 temp = other.transform.position - transform.position;
            Vector3 direction = temp.normalized;

            // Ray���΂�
            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(this.transform.position + rayPosition, ray.direction * distance, Color.red);

            // ����ۊ�
            RaycastHit hit;

            //�ŏ��ɓ��������I�u�W�F�N�g�𒲂ׂ�
            if (Physics.Raycast(this.transform.position + rayPosition, ray.direction * distance, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    player = other.transform;
                    agent.destination = player.position;
                    Debug.Log("�݂���" + player.position);
                    if (Physics.CheckSphere(transform.position, attackRad, LayerMask.GetMask("Player")))
                    {
                        // �����ɍU������
                        Debug.Log("�U���J�n�I");
                    }
                }
            }
            else
            {
                Debug.Log("�ǂ�����");
            }

        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = default;
            //player���͈͂̊O�ɏo���珉���ʒu�ɖ߂�
            agent.destination = rootPos;
            Debug.Log("�ǂ����s���������");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, attackRad);
    }
}
