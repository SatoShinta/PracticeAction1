using UnityEngine;
using UnityEngine.AI;

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float rad = 0f;
    [SerializeField] float attackRad = 0f;
    [SerializeField] Vector3 rootPos = Vector3.zero; // �����ʒu
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        rootPos = transform.position;
    }

    void Update()
    {
        // CheckPlayer();
    }

    //public void CheckPlayer()
    //{
    //    if ()
    //    {
    //        player = FindAnyObjectByType<PlayerController>().transform;
    //        agent.destination = player.position;
    //        Debug.Log("�݂���" + player.position);
    //    }
    //    else
    //    {
    //        player = default;
    //        //player���͈͂̊O�ɏo���珉���ʒu�ɖ߂�
    //        agent.destination = rootPos;
    //        Debug.Log("�ǂ����s���������");
    //    }
    //}


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Ray���΂��������v�Z
            Vector3 temp = other.transform.position - transform.position;
            Vector3 direction = temp.normalized;

            // Ray���΂�
            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(ray.origin, ray.direction);

            // ����ۊ�
            RaycastHit hit;

            //�ŏ��ɓ��������I�u�W�F�N�g�𒲂ׂ�
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    player = other.transform;
                    agent.destination = player.position;
                    Debug.Log("�݂���" + player.position);
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
