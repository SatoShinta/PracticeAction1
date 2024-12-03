using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ジャンプする処理を抜いた階段などの段差を上ることのできる操作コード

    [SerializeField, Header("動くスピード")] float moveSpeed = 10f;
    [SerializeField, Header("走ってる時のスピード")] float runSpeed = 10f;
    Rigidbody playerRigit = null;
    Animator playerAnim = null;
    InputSystem_Actions inputActions = null;
    Vector3 playerInput = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    public bool isGrounded = false; // 地面に立っているかどうか
    public bool isCollision = false; // 前方の壁に衝突しているかどうか
    public bool isDashing = false;

    [Space(20)]
    [Header("---接地確認用のコライダー設定---")]
    [SerializeField, Header("コライダーの位置")] Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
    [SerializeField, Header("コライダーの半径")] float groundColliderRadius = 0.29f;

    [Space(20)]
    [Header("---衝突確認用のコライダー設定---")]
    [SerializeField, Header("コライダーの位置")] Vector3 collisionPositionOffset = new Vector3(0f, 0.5f, 0f);
    [SerializeField, Header("コライダーの半径")] float collisionColliderRadius = 0.3f;

    [Space(20)]
    [Header("---段差確認用のレイ設定---")]
    [SerializeField, Header("レイを発生させる位置")] Vector3 stepRayOffset = new Vector3(0f, 0.05f, 0f);
    [SerializeField, Header("レイを飛ばす距離")] float stepDistance = 0.5f;
    [SerializeField, Header("登れる段差の高さ")] float stepOffset = 0.3f;
    [SerializeField, Header("登れる角度")] float slopeAngle = 65f;
    [SerializeField, Header("登れる段差の位置から飛ばすレイの距離")] float slopeDistance = 0.6f;


    private void Start()
    {
        playerRigit = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        inputActions = new InputSystem_Actions();

        // 移動入力をインプットシステムで行う
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

        // プレイヤーの向きをカメラに合わせる処理
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        // 移動方向を計算 （nomalizedで正規化し、斜め移動でスピードが上がらないようにしている）
        Vector3 movingDirection = horizontalRotation * new Vector3(playerInput.x, 0, playerInput.y).normalized;

        if (isGrounded)
        {
            velocity = Vector3.zero;

            // 方向キーの入力があった場合
            if (playerInput.magnitude > 0)
            {
                // プレイヤーの正面がカメラの向きになるような処理
                Quaternion targetRotation = Quaternion.LookRotation(movingDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
                //playerAnim.SetBool("isWalking",true);

                // プレイヤーの正面がカメラの向きになるような処理
                float currentSpeed = isDashing ? runSpeed : moveSpeed;
                velocity = movingDirection * currentSpeed * Time.fixedDeltaTime;

                var stepRayPosition = playerRigit.position + stepRayOffset;

                // ここで発生させたレイが段差に当たっているか確認
                if (Physics.Linecast(stepRayPosition, stepRayPosition + playerRigit.transform.forward * stepDistance, out var stepHit))
                {
                    /// slopeAngleの値を超えない段差か、
                    /// slopeAngleの値を超えているが、進行方向上にstepOffsetの高さより上にオブジェクトがなければ
                    /// この処理を行う
                    if (Vector3.Angle(playerRigit.transform.up, stepHit.normal) <= slopeAngle
                        || (Vector3.Angle(playerRigit.transform.up, stepHit.normal) > slopeAngle)
                        && (Physics.Linecast(playerRigit.position + new Vector3(0, stepOffset, 0f), playerRigit.position + new Vector3(0, stepOffset, 0f) + playerRigit.transform.forward * stepDistance)))
                    {
                        /// FromToRotationでプレイヤーの上方向に伸びている線から
                        /// レイが当たったオブジェクトの法線の交点の部分にできた角度を取得し、
                        /// 前方向に動く速さでその角度の分上に進む
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
    /// プレイヤーが地面と接触しているか確認するメソッド
    /// </summary>
    void CheckGround()
    {      // プレイヤーの現在のポジションからgroundPositionOffsetの値を追加した地点にgroundColliderRadiusの半径の大きさの球体を作り、
           // その球体がPlayerレイヤー以外のレイヤーに当たったら地面に立っている判定にする
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
        // 接地確認のギズモ
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
