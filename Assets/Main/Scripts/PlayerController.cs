using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("動くスピード")] float moveSpeed = 10f;
    [SerializeField, Header("走ってる時のスピード")] float runSpeed = 10f;
    Rigidbody playerRigit = null;
    bool isGrounded = false; // 地面に立っているかどうか
    bool isCollision = false; // 前方の壁に衝突しているかどうか

    [Space(20)]
    [Header("---接地確認用のコライダー設定---")]
    [SerializeField, Header("コライダーの位置")] Vector3 groundPositionOffset = new Vector3(0f, 0.02f, 0f);
    [SerializeField, Header("コライダーの半径")] float groundColliderRadius = 0.29f;

    [Space(20)]
    [Header("---衝突確認用のコライダー設定---")]
    [SerializeField, Header("衝突確認用のコライダー")] Collider collisionCollider = null;

    [Space(20)]
    [Header("---段差確認用のレイ設定---")]
    [SerializeField, Header("レイを発生させる位置")] Transform stepRay;
    [SerializeField, Header("レイを飛ばす距離")] float stepDistance = 0.5f;
    [SerializeField, Header("登れる段差の高さ")] float stepOffset = 0.3f;
    [SerializeField, Header("登れる角度")] float slopeAngle = 65f;
    [SerializeField, Header("登れる段差の位置から飛ばすレイの距離")] float slopeDistance = 0.6f;



    private void OnDrawGizmos()
    {
        // 接地確認のギズモ
    }
}
