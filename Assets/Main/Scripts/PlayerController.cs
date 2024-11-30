using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("�����X�s�[�h")] float moveSpeed = 10f;
    [SerializeField, Header("�����Ă鎞�̃X�s�[�h")] float runSpeed = 10f;
    [SerializeField, Header("���C���΂��ʒu")] Transform stepRay;
    [SerializeField, Header("����΂�����")] float stepDistance = 0.5f;
    [SerializeField, Header("�o���i��")] float stepOffset = 0.3f;
    [SerializeField, Header("�o���p�x")] float slopeLimit = 0.5f;
    [SerializeField, Header("�o���i���̈ʒu�����΂����C�̋���")] float slopeDistance = 0.3f;
    public int layerMask;
    public bool isOnStairs = false;
    public bool isOnGround = false;

    Rigidbody playerRigit = null;
    Vector3 movingVelocity = Vector3.zero;
    Vector3 cameraDirection = Vector3.zero;
    Animator playerAnim = null;


    void Start()
    {
        playerRigit = this.gameObject.GetComponent<Rigidbody>();
        playerAnim = this.gameObject.GetComponent<Animator>();
        layerMask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    void Update()
    {
        // ���͂��󂯂�
        var horizontal = Input.GetAxisRaw("Horizontal");
        var Vertical = Input.GetAxisRaw("Vertical");

        // �v���C���[�̌������J�����ɍ��킹�鏈��
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // �ړ��������v�Z�inormalized�Ő��K�����A�΂߈ړ��ŃX�s�[�h���オ��Ȃ��悤�ɂ��Ă���j
        Vector3 movingDirection = horizontalRotation * new Vector3(horizontal, 0, Vertical).normalized;


        // �ړ������ɑ��x�������Ĉړ����x���v�Z
        movingVelocity = movingDirection * moveSpeed;

        // �o���i����\��
        Debug.DrawLine(transform.position + new Vector3(0f, stepOffset, 0f),
            transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * slopeDistance,
            Color.green);

        // �v���C���[���ړ����Ă���Ƃ��A
        if (movingVelocity != Vector3.zero)
        {
            // �v���C���[�̐��ʂ��ړ������Ɠ����ɂ���
            Quaternion targetRotation = Quaternion.LookRotation(movingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            playerAnim.SetBool("isWalking", true);

            //// �o���i����\��
            //Debug.DrawLine(transform.position + new Vector3(0f, stepOffset, 0f),
            //    transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * slopeDistance,
            //    Color.green);

            if (Physics.Linecast(transform.position, stepRay.position + stepRay.forward * stepDistance, out var stepHit, LayerMask.GetMask("Field", "Stairs")))
            {
                float slopeThreshold = (Input.GetKey(KeyCode.LeftShift)) ? slopeLimit + 0.2f : slopeLimit;
                isOnStairs = true;

                if (Vector3.Angle(transform.up, stepHit.normal) <= slopeThreshold
                    || (Vector3.Angle(transform.up, stepHit.normal) > slopeThreshold
                    || !Physics.Linecast(transform.position + new Vector3(0f, stepOffset, 0f), transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * slopeDistance, LayerMask.GetMask("Stairs"))))
                {
                    float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed + 0.2f : moveSpeed;

                    movingVelocity = new Vector3(0f, (Quaternion.FromToRotation(Vector3.up, stepHit.normal) * transform.forward * currentSpeed).y, currentSpeed);
                    Debug.Log(Vector3.Angle(transform.up, stepHit.normal));
                }
            }
            else
            {
                isOnStairs = false;
            }


            // ���胂�[�V����
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerAnim.SetBool("isRun", true);
                movingVelocity = movingDirection * runSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                playerAnim.SetBool("isRun", false);
            }
        }
        else
        {
            playerRigit.linearVelocity = Vector3.zero;
            playerRigit.angularVelocity = Vector3.zero;
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isRun", false);
        }

    }

    private void FixedUpdate()
    {
        if (!isOnStairs && !isOnGround)
        {
            playerRigit.useGravity = true;
        }

        // �ړ�����
        playerRigit.linearVelocity = new Vector3(movingVelocity.x, movingVelocity.y, movingVelocity.z);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

}
