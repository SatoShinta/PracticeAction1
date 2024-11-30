using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("動くスピード")] float moveSpeed = 10f;
    [SerializeField, Header("走ってる時のスピード")] float runSpeed = 10f;
    [SerializeField, Header("レイを飛ばす位置")] Transform stepRay;
    [SerializeField, Header("例を飛ばす距離")] float stepDistance = 0.5f;
    [SerializeField, Header("登れる段差")] float stepOffset = 0.3f;
    [SerializeField, Header("登れる角度")] float slopeLimit = 0.5f;
    [SerializeField, Header("登れる段差の位置から飛ばすレイの距離")] float slopeDistance = 0.3f;
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
        // 入力を受けつけ
        var horizontal = Input.GetAxisRaw("Horizontal");
        var Vertical = Input.GetAxisRaw("Vertical");

        // プレイヤーの向きをカメラに合わせる処理
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // 移動方向を計算（normalizedで正規化し、斜め移動でスピードが上がらないようにしている）
        Vector3 movingDirection = horizontalRotation * new Vector3(horizontal, 0, Vertical).normalized;


        // 移動方向に速度をかけて移動速度を計算
        movingVelocity = movingDirection * moveSpeed;

        // 登れる段差を表示
        Debug.DrawLine(transform.position + new Vector3(0f, stepOffset, 0f),
            transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * slopeDistance,
            Color.green);

        // プレイヤーが移動しているとき、
        if (movingVelocity != Vector3.zero)
        {
            // プレイヤーの正面を移動方向と同じにする
            Quaternion targetRotation = Quaternion.LookRotation(movingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            playerAnim.SetBool("isWalking", true);

            //// 登れる段差を表示
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


            // 走りモーション
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

        // 移動処理
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
