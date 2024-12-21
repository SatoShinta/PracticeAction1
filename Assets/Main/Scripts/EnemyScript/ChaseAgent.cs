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


    /// <summary>
    /// OnTriggerStay�Ɠ������Ƃ����Ă��郁�\�b�h�i�X�e�[�g�}�V���Ŏg�p���邽�߂ɍ�����j
    /// </summary>
    public void OncolliderStay()
    {
        // OverlapBox���g�p���āA���̃I�u�W�F�N�g�ɐݒ肳��Ă�����G�p�R���C�_�[�͈͓̔��ɑ��݂���A"Player"���C���[�����������ׂẴR���C�_�[��colliders�z��Ɋi�[����
        Collider[] colliders = Physics.OverlapBox(searchCollider.bounds.center, searchCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Player"));
        bool playeIsInside = false; // �v���C���[�����ɍ��G�͈͓��ɂ��邩�ǂ����̃t���O

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                playeIsInside = true;
                SearchForEnemies(col);
            }
           
        }
        if (!playeIsInside)
        {
            agent.destination = rootPos;
            Debug.Log("�A��܂�");
        }

    }

    public void SearchForEnemies(Collider col)
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, attackRad);
    }
}
