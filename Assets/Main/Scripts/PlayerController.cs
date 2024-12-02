using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("�����X�s�[�h")] float moveSpeed = 10f;
    [SerializeField, Header("�����Ă鎞�̃X�s�[�h")] float runSpeed = 10f;
    Rigidbody playerRigit = null;
    bool isGrounded = false; // �n�ʂɗ����Ă��邩�ǂ���
    bool isCollision = false; // �O���̕ǂɏՓ˂��Ă��邩�ǂ���

    [Space(20)]
    [Header("---�ڒn�m�F�p�̃R���C�_�[�ݒ�---")]
    [SerializeField, Header("�R���C�_�[�̈ʒu")] Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
    [SerializeField, Header("�R���C�_�[�̔��a")] float groundColliderRadius = 0.29f;

    [Space(20)]
    [Header("---�Փˊm�F�p�̃R���C�_�[�ݒ�---")]
    [SerializeField, Header("�Փˊm�F�p�̃R���C�_�[")] Collider collisionCollider = null;

    [Space(20)]
    [Header("---�i���m�F�p�̃��C�ݒ�---")]
    [SerializeField, Header("���C�𔭐�������ʒu")] Transform stepRay;
    [SerializeField, Header("���C���΂�����")] float stepDistance = 0.5f;
    [SerializeField, Header("�o���i���̍���")] float stepOffset = 0.3f;
    [SerializeField, Header("�o���p�x")] float slopeAngle = 65f;
    [SerializeField, Header("�o���i���̈ʒu�����΂����C�̋���")] float slopeDistance = 0.6f;



    private void OnDrawGizmos()
    {
        // �ڒn�m�F�̃M�Y��
    }
}
