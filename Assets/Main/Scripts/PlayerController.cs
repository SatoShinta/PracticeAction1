using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("動くスピード")] float moveSpeed = 10f;
    Rigidbody playerRigit = null;
    Vector3 movingVelocity = Vector3.zero;

    void Start()
    {
        playerRigit = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var Vertical = Input.GetAxisRaw("Vertical");

        Vector3 movingDirection = new Vector3(horizontal, 0, Vertical).normalized;

        movingVelocity = movingDirection * moveSpeed;
    }

    private void FixedUpdate()
    {
        playerRigit.AddForce(movingVelocity);
    }
}
