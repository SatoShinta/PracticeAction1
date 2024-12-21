using UnityEngine;
using UnityEngine.AI;

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float attackRad = 0f;
    [SerializeField] Vector3 rootPos = Vector3.zero; // 初期位置
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
            // Rayを飛ばす方向を計算
            Vector3 temp = other.transform.position - transform.position;
            Vector3 direction = temp.normalized;

            // Rayを飛ばす
            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(this.transform.position + rayPosition, ray.direction * distance, Color.red);

            // 情報を保管
            RaycastHit hit;

            //最初に当たったオブジェクトを調べる
            if (Physics.Raycast(this.transform.position + rayPosition, ray.direction * distance, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    player = other.transform;
                    agent.destination = player.position;
                    Debug.Log("みつけた" + player.position);
                    if (Physics.CheckSphere(transform.position, attackRad, LayerMask.GetMask("Player")))
                    {
                        // ここに攻撃処理
                        Debug.Log("攻撃開始！");
                    }
                }
            }
            else
            {
                Debug.Log("壁がある");
            }

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
        Gizmos.DrawSphere(transform.position, attackRad);
    }
}
