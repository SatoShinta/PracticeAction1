using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyLockOn : MonoBehaviour
{
    [SerializeField,Header("敵の情報")] List<GameObject> enemyList = new List<GameObject>();
    [SerializeField,Header("索敵範囲")] float lookOnColliderRadius = 10f;
    [SerializeField,Header("索敵範囲の限界値")] float lookOnColliderMaxDistance = 10f;

    void Start()
    {

    }

    void Update()
    {
        if (Physics.CheckSphere(transform.position, lookOnColliderRadius, LayerMask.GetMask("Enemy")))
        {
            LookOnTarget();
        }

    }

    void LookOnTarget()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, lookOnColliderRadius, transform.position, lookOnColliderMaxDistance, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hit in hits)
        {
            GameObject enemy = hit.collider.gameObject;
            if (!enemyList.Contains(enemy))
            {
                enemyList.Add(enemy);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // スフィアキャストの範囲を描写
        Gizmos.DrawWireSphere(transform.position, lookOnColliderRadius);

        // スフィアキャストの方向を示すために線を描写
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * lookOnColliderMaxDistance);
    }
}
