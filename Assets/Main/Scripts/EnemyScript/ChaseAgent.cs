using UnityEngine;
using UnityEngine.AI;

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float rad = 0f;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    void Update()
    {
        CheckPlayer();
        if (agent.enabled)
        {
            agent.destination = player.transform.position;
        }
    }

    public void CheckPlayer()
    {
        if (Physics.CheckSphere(transform.position, rad, LayerMask.GetMask("Player")))
        {
            transform.LookAt(player.transform);
            agent.enabled = true;
        }
        else
        {
            agent.enabled = false;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position ,rad);
    }
}
