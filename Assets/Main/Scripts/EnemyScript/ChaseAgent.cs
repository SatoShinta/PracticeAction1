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
    //    if (Physics.CheckSphere(transform.position, rad, LayerMask.GetMask("Player")))
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
            player = FindAnyObjectByType<PlayerController>().transform;
            agent.destination = player.position;
            Debug.Log("�݂���" + player.position);
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
        Gizmos.DrawWireSphere(transform.position, rad);
        Gizmos.DrawSphere(transform.position, attackRad);
    }
}
