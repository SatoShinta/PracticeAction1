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
    bool isGrounded = false; // 地面に立っているかどうか
    bool isCollision = false; // 前方の壁に衝突しているかどうか

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
    [SerializeField, Header("レイを発生させる位置")] Transform stepRay;
    [SerializeField, Header("レイを飛ばす距離")] float stepDistance = 0.5f;
    [SerializeField, Header("登れる段差の高さ")] float stepOffset = 0.3f;
    [SerializeField, Header("登れる角度")] float slopeAngle = 65f;
    [SerializeField, Header("登れる段差の位置から飛ばすレイの距離")] float slopeDistance = 0.6f;


    private void Start()
    {
        playerRigit = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.started += OnMove;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Enable();
    }

    private void Update()
    {
        CheckGround();
        //入力を受け付ける
    }


    private void FixedUpdate()
    {
        velocity = new Vector3(playerInput.x, 0f, playerInput.y) * moveSpeed * Time.fixedDeltaTime;
        playerRigit.MovePosition(playerRigit.position + velocity);
    }


    void OnMove(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
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
    }
}
