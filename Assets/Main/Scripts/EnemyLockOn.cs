using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    [SerializeField, Header("�G�̏��")] List<GameObject> enemyList = new List<GameObject>(); //�@���HashSet�ɕύX����
    [SerializeField, Header("���G�͈�")] float lookOnColliderRadius = 10f;
    [SerializeField, Header("���G�͈͂̌��E�l")] float lookOnColliderMaxDistance = 10f;
    [SerializeField, Header("���݂̃^�[�Q�b�g")] GameObject currentTargetEnemy = null;
    InputSystem_Actions inputAction = null;

    public bool isLockOn = false; // �G�����b�N�I�����Ă��邩�ǂ���

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
            //�@���b�N�I�������G�Ɍ������Đ��ʂ�����
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
    /// ���b�N�I���p�ɓG�̏����擾���郁�\�b�h
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
    /// ���b�N�I���͈̔͊O�ɂȂ����G��List����폜���郁�\�b�h
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
    /// �G�̕�������Ɍ����悤�ɂ��郁�\�b�h
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
        // �X�t�B�A�L���X�g�͈̔͂�`��
        Gizmos.DrawWireSphere(transform.position, lookOnColliderRadius);

        // �X�t�B�A�L���X�g�̕������������߂ɐ���`��
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * lookOnColliderMaxDistance);
    }

    private void OnDestroy()
    {
        inputAction?.Dispose();
    }
}
