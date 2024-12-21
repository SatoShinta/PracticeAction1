using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Collider searchCollider;
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

    private void Update()
    {
        OncolliderStay();
    }


    public void OnTriggerStay(Collider other)
    {

    }

    //public void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        player = default;
    //        //player���͈͂̊O�ɏo���珉���ʒu�ɖ߂�
    //        agent.destination = rootPos;
    //        Debug.Log("�ǂ����s���������");
    //    }
    //}

    public void OncolliderStay()
    {
        Collider[] colliders = Physics.OverlapBox(searchCollider.bounds.center, searchCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Player"));
        bool playeIsInside = false;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                playeIsInside = true;
                Vector3 direction = (col.transform.position - transform.position).normalized;

                Ray ray = new Ray(transform.position + rayPosition, direction);
                Debug.DrawRay(this.transform.position + rayPosition, ray.direction, Color.red);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        player = hit.transform;
                        agent.destination = player.position;
                        Debug.Log("������" + player.position);

                        if (Physics.CheckSphere(transform.position, attackRad, LayerMask.GetMask("Player")))
                        {
                            // �����ɍU������
                            Debug.Log("�U���I�I�I");
                        }
                    }
                    else
                    {
                        Debug.Log("�ǂ�����");
                    }
                }
                
            }
           
        }
        if (!playeIsInside)
        {
            agent.destination = rootPos;
            Debug.Log("�A��܂�");
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, attackRad);
    }
}
