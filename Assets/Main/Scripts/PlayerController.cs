using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("動くスピード")] float moveSpeed = 10f;
    Rigidbody playerRigit = null;
    Vector3 movingVelocity = Vector3.zero;
    Animator playerAnim = null;

    void Start()
    {
        playerRigit = this.gameObject.GetComponent<Rigidbody>();
        playerAnim = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var Vertical = Input.GetAxisRaw("Vertical");

        // 移動方向を計算（normalizedで正規化し、斜め移動でスピードが上がらないようにしている）
        Vector3 movingDirection = new Vector3(horizontal, 0, Vertical).normalized;

        // 移動方向に速度をかけて移動速度を計算
        movingVelocity = movingDirection * moveSpeed;
        playerRigit.linearVelocity = new Vector3(movingVelocity.x, movingVelocity.y, movingVelocity.z);

        // プレイヤーが移動しているとき、
        if (movingVelocity != Vector3.zero)
        {
            // プレイヤーの正面を移動方向と同じにする
            Quaternion targetRotation = Quaternion.LookRotation(movingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            playerAnim.SetBool("isWalking", true);
        }
        else
        {
            playerRigit.linearVelocity = Vector3.zero;
            playerRigit.angularVelocity = Vector3.zero;
            playerAnim.SetBool("isWalking", false);
        }


    }

    private void FixedUpdate()
    {
        // 移動処理
        playerRigit.AddForce(movingVelocity, ForceMode.Force);
    }
}
