using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("�����X�s�[�h")] float moveSpeed = 10f;
    Rigidbody playerRigit = null;
    Vector3 movingVelocity = Vector3.zero;

    void Start()
    {
        playerRigit = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var Vertical = Input.GetAxisRaw("Vertical");

        // �ړ��������v�Z�inormalized�Ő��K�����A�΂߈ړ��ŃX�s�[�h���オ��Ȃ��悤�ɂ��Ă���j
        Vector3 movingDirection = new Vector3(horizontal, 0, Vertical).normalized;

        // �ړ������ɑ��x�������Ĉړ����x���v�Z
        movingVelocity = movingDirection * moveSpeed;

        // �v���C���[���ړ����Ă���Ƃ��A
        if (movingVelocity != Vector3.zero)
        {
            // �v���C���[�̐��ʂ��ړ������Ɠ����ɂ���
            Quaternion targetRotation = Quaternion.LookRotation(movingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }

    }

    private void FixedUpdate()
    {
        // �ړ�����
        playerRigit.AddForce(movingVelocity, ForceMode.Force);
    }
}
