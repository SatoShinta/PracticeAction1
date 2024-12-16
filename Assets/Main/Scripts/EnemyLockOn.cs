using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    [SerializeField, Header("敵の情報")] List<GameObject> enemyList = new List<GameObject>(); //　後でHashSetに変更する
    [SerializeField, Header("索敵範囲")] float lookOnColliderRadius = 10f;
    [SerializeField, Header("索敵範囲の限界値")] float lookOnColliderMaxDistance = 10f;
    [SerializeField, Header("現在のターゲット")] GameObject currentTargetEnemy = null;
    InputSystem_Actions inputAction = null;

    public bool isLockOn = false; // 敵をロックオンしているかどうか

    PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        inputAction = new InputSystem_Actions();


        inputAction.Enable();
    }

    void Update()
    {
        LockOnTarget();
        RemoveLockOnTarget();
        OnLockOn();
        OnLockOnCanceled();

        if (isLockOn && currentTargetEnemy != null)
        {
            //　ロックオンした敵に向かって正面を向く
            transform.LookAt(currentTargetEnemy.transform);
        }
    }

    void OnLockOn()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && enemyList != null)
        {
            isLockOn = true;
            LookAtTarget();
        }

    }

    void OnLockOnCanceled()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isLockOn = false;
            currentTargetEnemy = null;
        }
    }


    /// <summary>
    /// ロックオン用に敵の情報を取得するメソッド
    /// </summary>
    void LockOnTarget()
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

    /// <summary>
    /// ロックオンの範囲外になった敵をListから削除するメソッド
    /// </summary>
    void RemoveLockOnTarget()
    {
        List<GameObject> enemyRemoveList = new List<GameObject>();

        foreach (GameObject enemy in enemyList)
        {
            if (!Physics.CheckSphere(transform.position, lookOnColliderRadius, LayerMask.GetMask("Enemy")) || enemy == null)
            {
                enemyRemoveList.Add(enemy);
            }

        }

        foreach (GameObject enemy in enemyRemoveList)
        {
            enemyList.Remove(enemy);
        }

    }


    /// <summary>
    /// 敵の方向を常に向くようにするメソッド
    /// </summary>
    public void LookAtTarget()
    {
        if (enemyList.Count > 0)
        {
            currentTargetEnemy = enemyList.OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).FirstOrDefault();
            if (enemyList.Count < 0)
            {
                currentTargetEnemy = null;
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

    private void OnDestroy()
    {
        inputAction?.Dispose();
    }
}
