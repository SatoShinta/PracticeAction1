using UnityEngine;

public class PlayerConTest : MonoBehaviour
{
    private Animator animator;
    private Vector3 velocity;
    [SerializeField]
    private float jumpPower = 5f;
    //　入力値
    private Vector3 input;
    //　歩く速さ
    [SerializeField]
    private float walkSpeed = 4f;
    //　rigidbody
    private Rigidbody rigid;
    //　地面に接地しているかどうか
    [SerializeField]
    private bool isGrounded;
    //　前方の壁に衝突しているかどうか
    [SerializeField]
    private bool isCollision;
    //　接地確認のコライダの位置のオフセット
    [SerializeField]
    private Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
    //　接地確認の球のコライダの半径
    [SerializeField]
    private float groundColliderRadius = 0.29f;
    //　衝突確認のコライダの位置のオフセット
    [SerializeField]
    private Vector3 collisionPositionOffset = new Vector3(0f, 0.5f, 0.1f);
    //　衝突確認の球のコライダの半径
    [SerializeField]
    private float collisionColliderRadius = 0.3f;
    //　ジャンプ中かどうか
    [SerializeField]
    private bool isJump;
    //　ジャンプ後の着地判定までの遅延時間
    [SerializeField]
    private float delayTimeToLanding = 0.5f;
    //　ジャンプ後の時間
    [SerializeField]
    private float jumpTime;

    //　前方に段差があるか調べるレイを飛ばすオフセット位置
    [SerializeField]
    private Vector3 stepRayOffset = new Vector3(0f, 0.05f, 0f);
    //　レイを飛ばす距離
    [SerializeField]
    private float stepDistance = 0.5f;
    //　昇れる段差
    [SerializeField]
    private float stepOffset = 0.3f;
    //　昇れる角度
    [SerializeField]
    private float slopeLimit = 65f;
    //　昇れる段差の位置から飛ばすレイの距離
    [SerializeField]
    private float slopeDistance = 0.6f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGround();
        //　キャラクターが接地している場合
        if (isGrounded)
        {
            //　接地したので移動速度を0にする
            velocity = Vector3.zero;
            input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //　方向キーが多少押されている
            if (input.magnitude > 0f)
            {
                animator.SetFloat("Speed", input.magnitude);
                transform.LookAt(rigid.position + input);

                var stepRayPosition = rigid.position + stepRayOffset;

                //　ステップ用のレイが地面に接触しているかどうか
                if (Physics.Linecast(stepRayPosition, stepRayPosition + rigid.transform.forward * stepDistance, out var stepHit))
                {
                    //　進行方向の地面の角度が指定以下、または昇れる段差より下だった場合の移動処理

                    if (Vector3.Angle(rigid.transform.up, stepHit.normal) <= slopeLimit
                    || (Vector3.Angle(rigid.transform.up, stepHit.normal) > slopeLimit
                        && !Physics.Linecast(rigid.position + new Vector3(0f, stepOffset, 0f), rigid.position + new Vector3(0f, stepOffset, 0f) + rigid.transform.forward * slopeDistance))
                    )
                    {
                        velocity = new Vector3(0f, (Quaternion.FromToRotation(Vector3.up, stepHit.normal) * rigid.transform.forward * walkSpeed).y, 0f) + rigid.transform.forward * walkSpeed;
                    }
                    else
                    {
                        //　指定した条件に当てはまらない場合は速度を0にする
                        velocity = Vector3.zero;
                    }

                    Debug.Log(Vector3.Angle(Vector3.up, stepHit.normal));

                    //　前方の壁に接触していなければ
                }
                else
                {
                    velocity = transform.forward * walkSpeed;
                }
                //　キーの押しが小さすぎる場合は移動しない
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }
            //　ジャンプ
            if (Input.GetButtonDown("Jump"))
            {
                //　ジャンプしたら接地していない状態にする
                isGrounded = false;
                isJump = true;
                jumpTime = 0f;
                velocity.y = jumpPower;
                // 2ax = v²-v₀²より
                //velocity.y = Mathf.Sqrt(-2 * Physics.gravity.y * jumpPower);
                animator.SetBool("Jump", true);
            }
        }
        //　接触していたら移動方向の値は0にする
        if (!isGrounded && isCollision)
        {
            velocity = new Vector3(0f, velocity.y, 0f);
        }
        //　ジャンプ時間の計算
        if (isJump && jumpTime < delayTimeToLanding)
        {
            jumpTime += Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        //　キャラクターを移動させる処理
        rigid.MovePosition(rigid.position + velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //　指定したコライダと接触、かつ接触確認コライダと接触していたら衝突状態にする
        if (Physics.CheckSphere(rigid.position + transform.up * collisionPositionOffset.y + transform.forward * collisionPositionOffset.z, collisionColliderRadius, ~LayerMask.GetMask("Player"))
            )
        {
            isCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //　指定したコライダと離れたら衝突していない状態にする
        isCollision = false;
    }

    //　地面のチェック
    private void CheckGround()
    {
        //　地面に接地しているか確認
        if (Physics.CheckSphere(rigid.position + groundPositionOffset, groundColliderRadius, ~LayerMask.GetMask("Player")))
        {
            //　ジャンプ中
            if (isJump)
            {
                if (jumpTime >= delayTimeToLanding)
                {
                    isGrounded = true;
                    isJump = false;
                }
                else
                {
                    isGrounded = false;
                }
            }
            else
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }
        animator.SetBool("Jump", !isGrounded);
    }

    private void OnDrawGizmos()
    {
        //　接地確認のギズモ
        Gizmos.DrawWireSphere(transform.position + groundPositionOffset, groundColliderRadius);
        Gizmos.color = Color.blue;
        //　衝突確認のギズモ
        Gizmos.DrawWireSphere(transform.position + transform.up * collisionPositionOffset.y + transform.forward * collisionPositionOffset.z, collisionColliderRadius);
        var stepRayPosition = transform.position + stepRayOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(stepRayPosition, stepRayPosition + transform.forward * stepDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0f, stepOffset, 0f), transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * slopeDistance);
    }
}


