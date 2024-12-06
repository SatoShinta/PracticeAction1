using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyLockOn : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] float lookOnColliderRadius = 10f;
    [SerializeField] float lookOnColliderMaxDistance = 10f;

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
        // �X�t�B�A�L���X�g�͈̔͂�`��
        Gizmos.DrawWireSphere(transform.position, lookOnColliderRadius);

        // �X�t�B�A�L���X�g�̕������������߂ɐ���`��
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * lookOnColliderMaxDistance);
    }
}
