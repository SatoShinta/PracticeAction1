using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("動くスピード")] float moveSpeed = 10f;
    [SerializeField, Header("走ってる時のスピード")] float runSpeed = 10f;
    [SerializeField, Header("レイを飛ばす位置")] Transform stepRay;
    [SerializeField, Header("例を飛ばす距離")] float stepDistance = 0.5f;
    [SerializeField, Header("登れる段差")] float stepOffset = 0.3f;
    [SerializeField, Header("登れる角度")] float slopeLimit = 0.3f;
    [SerializeField, Header("登れる段差の位置から飛ばすレイの距離")] float slopeDistance = 0.3f;
    public int layerMask;

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


        // プレイヤーが移動しているとき、
        if (movingVelocity != Vector3.zero)
        {
            // プレイヤーの正面を移動方向と同じにする
            Quaternion targetRotation = Quaternion.LookRotation(movingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            playerAnim.SetBool("isWalking", true);

            // 登れる段差を表示
            Debug.DrawLine(transform.position + new Vector3(0f, stepOffset, 0f), transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * slopeDistance, Color.green);
            if(Physics.Linecast(transform.position,stepRay.position +stepRay.forward * stepDistance, out var stepHit, LayerMask.GetMask("Field", "Block")))
            {
                if (Vector3.Angle(transform.up, stepHit.normal) <= slopeLimit
                    || (Vector3.Angle(transform.up, stepHit.normal) > slopeLimit
                    && !Physics.Linecast(transform.position + new Vector3(0f, stepOffset, 0f), transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * slopeDistance, LayerMask.GetMask("Field", "Block"))))
                {
                    movingVelocity = new Vector3(0f, (Quaternion.FromToRotation(Vector3.up, stepHit.normal) * transform.forward * moveSpeed).y) + transform.forward * moveSpeed;
                    Debug.Log(Vector3.Angle(transform.up, stepHit.normal));
                }
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
        // 移動処理
        playerRigit.linearVelocity = new Vector3(movingVelocity.x, movingVelocity.y, movingVelocity.z);
    }



}
