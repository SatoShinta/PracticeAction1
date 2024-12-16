using UnityEngine;
using UnityEngine.AI;

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.destination = player.transform.position;
    }
}
