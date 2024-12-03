using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // �W�����v���鏈���𔲂����K�i�Ȃǂ̒i������邱�Ƃ̂ł��鑀��R�[�h

    [SerializeField, Header("�����X�s�[�h")] float moveSpeed = 10f;
    [SerializeField, Header("�����Ă鎞�̃X�s�[�h")] float runSpeed = 10f;
    Rigidbody playerRigit = null;
    Animator playerAnim = null;
    InputSystem_Actions inputActions = null;
    Vector3 playerInput = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    public bool isGrounded = false; // �n�ʂɗ����Ă��邩�ǂ���
    public bool isCollision = false; // �O���̕ǂɏՓ˂��Ă��邩�ǂ���
    public bool isDashing = false;

    [Space(20)]
    [Header("---�ڒn�m�F�p�̃R���C�_�[�ݒ�---")]
    [SerializeField, Header("�R���C�_�[�̈ʒu")] Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
    [SerializeField, Header("�R���C�_�[�̔��a")] float groundColliderRadius = 0.29f;

    [Space(20)]
    [Header("---�Փˊm�F�p�̃R���C�_�[�ݒ�---")]
    [SerializeField, Header("�R���C�_�[�̈ʒu")] Vector3 collisionPositionOffset = new Vector3(0f, 0.5f, 0f);
    [SerializeField, Header("�R���C�_�[�̔��a")] float collisionColliderRadius = 0.3f;

    [Space(20)]
    [Header("---�i���m�F�p�̃��C�ݒ�---")]
    [SerializeField, Header("���C�𔭐�������ʒu")] Vector3 stepRayOffset = new Vector3(0f, 0.05f, 0f);
    [SerializeField, Header("���C���΂�����")] float stepDistance = 0.5f;
    [SerializeField, Header("�o���i���̍���")] float stepOffset = 0.3f;
    [SerializeField, Header("�o���p�x")] float slopeAngle = 65f;
    [SerializeField, Header("�o���i���̈ʒu�����΂����C�̋���")] float slopeDistance = 0.6f;


    private void Start()
    {
        playerRigit = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        inputActions = new InputSystem_Actions();

        // �ړ����͂��C���v�b�g�V�X�e���ōs��
        inputActions.Player.Move.started += OnMove;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Sprint.performed += OnDash;
        inputActions.Player.Sprint.canceled += OnDashCanceled;

        inputActions.Enable();
    }

    private void Update()
    {
        CheckGround();

        // �v���C���[�̌������J�����ɍ��킹�鏈��
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        // �ړ��������v�Z �inomalized�Ő��K�����A�΂߈ړ��ŃX�s�[�h���オ��Ȃ��悤�ɂ��Ă���j
        Vector3 movingDirection = horizontalRotation * new Vector3(playerInput.x, 0, playerInput.y).normalized;

        if (isGrounded)
        {
            velocity = Vector3.zero;

            // �����L�[�̓��͂��������ꍇ
            if (playerInput.magnitude > 0)
            {
                // �v���C���[�̐��ʂ��J�����̌����ɂȂ�悤�ȏ���
                Quaternion targetRotation = Quaternion.LookRotation(movingDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
                //playerAnim.SetBool("isWalking",true);

                // �v���C���[�̐��ʂ��J�����̌����ɂȂ�悤�ȏ���
                float currentSpeed = isDashing ? runSpeed : moveSpeed;
                velocity = movingDirection * currentSpeed * Time.fixedDeltaTime;

                var stepRayPosition = playerRigit.position + stepRayOffset;

                // �����Ŕ������������C���i���ɓ������Ă��邩�m�F
                if (Physics.Linecast(stepRayPosition, stepRayPosition + playerRigit.transform.forward * stepDistance, out var stepHit))
                {
                    /// slopeAngle�̒l�𒴂��Ȃ��i�����A
                    /// slopeAngle�̒l�𒴂��Ă��邪�A�i�s�������stepOffset�̍�������ɃI�u�W�F�N�g���Ȃ����
                    /// ���̏������s��
                    if (Vector3.Angle(playerRigit.transform.up, stepHit.normal) <= slopeAngle
                        || (Vector3.Angle(playerRigit.transform.up, stepHit.normal) > slopeAngle)
                        && (Physics.Linecast(playerRigit.position + new Vector3(0, stepOffset, 0f), playerRigit.position + new Vector3(0, stepOffset, 0f) + playerRigit.transform.forward * stepDistance)))
                    {
                        /// FromToRotation�Ńv���C���[�̏�����ɐL�тĂ��������
                        /// ���C�����������I�u�W�F�N�g�̖@���̌�_�̕����ɂł����p�x���擾���A
                        /// �O�����ɓ��������ł��̊p�x�̕���ɐi��
                        velocity = new Vector3(0, (Quaternion.FromToRotation(Vector3.up, stepHit.normal) * playerRigit.transform.forward * moveSpeed).y,0f);
                    }
                }

            }
        }

    }


    private void FixedUpdate()
    {
        playerRigit.MovePosition(playerRigit.position + velocity);
    }


    void OnMove(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }


    void OnDash(InputAction.CallbackContext context)
    {
        isDashing = true;
    }

    void OnDashCanceled(InputAction.CallbackContext context)
    {
        isDashing = false;
    }

    /// <summary>
    /// �v���C���[���n�ʂƐڐG���Ă��邩�m�F���郁�\�b�h
    /// </summary>
    void CheckGround()
    {      // �v���C���[�̌��݂̃|�W�V��������groundPositionOffset�̒l��ǉ������n�_��groundColliderRadius�̔��a�̑傫���̋��̂����A
           // ���̋��̂�Player���C���[�ȊO�̃��C���[�ɓ���������n�ʂɗ����Ă��锻��ɂ���
        if (Physics.CheckSphere(playerRigit.position + groundPositionOffset, groundColliderRadius, ~LayerMask.GetMask("Player")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        // �ڒn�m�F�̃M�Y��
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + groundPositionOffset, groundColliderRadius);

        var stepRayPosition = transform.position + stepRayOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayPosition, stepRayPosition + transform.forward * stepDistance);
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }
}
