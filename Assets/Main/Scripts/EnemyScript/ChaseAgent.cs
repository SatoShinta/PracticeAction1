using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Collider searchCollider;
    [SerializeField] float attackRad = 0f;
    [SerializeField] Vector3 rootPos = Vector3.zero; // 初期位置
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
    /// OnTriggerStayと同じことをしているメソッド（ステートマシンで使用するために作った）
    /// </summary>
    public void OncolliderStay()
    {
        // OverlapBoxを使用して、このオブジェクトに設定されている索敵用コライダーの範囲内に存在する、"Player"レイヤーを持ったすべてのコライダーをcolliders配列に格納する
        Collider[] colliders = Physics.OverlapBox(searchCollider.bounds.center, searchCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("Player"));
        bool playeIsInside = false; // プレイヤーが中に索敵範囲内にいるかどうかのフラグ

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
            Debug.Log("帰ります");
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
                Debug.Log("見つけた" + player.position);

                if (Physics.CheckSphere(transform.position, attackRad, LayerMask.GetMask("Player")))
                {
                    // ここに攻撃処理
                    Debug.Log("攻撃！！！");
                }
            }
            else
            {
                Debug.Log("壁がある");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, attackRad);
    }
}
