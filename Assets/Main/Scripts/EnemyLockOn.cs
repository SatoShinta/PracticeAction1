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
    PlayerController playerController;

    public bool isLockOn = false; // �G�����b�N�I�����Ă��邩�ǂ���

    
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

    /// <summary>
    /// ���R���g���[���L�[����������A��ԋ߂��G�����b�N�I�����郁�\�b�h
    /// </summary>
    void OnLockOn()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && enemyList != null)
        {
            isLockOn = true;
            LookAtTarget();
        }

    }

    /// <summary>
    /// �R���g���[���L�[�𗣂�����A���b�N�I�����������郁�\�b�h
    /// </summary>
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
        // �����̎���ɓG��������A���̓G�̏����擾����
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, lookOnColliderRadius, transform.position, lookOnColliderMaxDistance, LayerMask.GetMask("Enemy")); 

        // �G�̏����擾������A�����List�Ɋi�[����i�d���͂����Ȃ��j
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

        // �G�����G�͈͊O�ɏo����A�������UenemyRemoveList�Ɋi�[����
        foreach (GameObject enemy in enemyList)
        {
            if (!Physics.CheckSphere(transform.position, lookOnColliderRadius, LayerMask.GetMask("Enemy")) || enemy == null)
            {
                enemyRemoveList.Add(enemy);
            }

        }

        // enemyRemoveList�̒��Ɋi�[����Ă�����̂��AenemyList�̒�����폜����
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
            // enemyList�̒��ɂ���G�l�~�[�̒��ŁAplayer�ƈ�ԋ������߂��G�l�~�[�����b�N�I���Ώۂɂ���
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
