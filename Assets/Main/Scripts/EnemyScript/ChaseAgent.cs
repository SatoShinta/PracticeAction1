using UnityEngine;
using UnityEngine.AI;

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float rad = 0f;
    [SerializeField] float attackRad = 0f;
    [SerializeField] Vector3 rootPos = Vector3.zero; // 初期位置
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
    //        Debug.Log("みつけた" + player.position);
    //    }
    //    else
    //    {
    //        player = default;
    //        //playerが範囲の外に出たら初期位置に戻る
    //        agent.destination = rootPos;
    //        Debug.Log("どっか行っちゃった");
    //    }
    //}


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = FindAnyObjectByType<PlayerController>().transform;
            agent.destination = player.position;
            Debug.Log("みつけた" + player.position);
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = default;
            //playerが範囲の外に出たら初期位置に戻る
            agent.destination = rootPos;
            Debug.Log("どっか行っちゃった");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rad);
        Gizmos.DrawSphere(transform.position, attackRad);
    }
}
