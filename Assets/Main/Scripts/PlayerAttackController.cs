using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    InputSystem_Actions inputAction;
    Animator playerAnim;
    Rigidbody playerRigit;
    PlayerController playerController;
    AnimatorClipInfo[] clipInfo;

    bool isAttack = false;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        inputAction = new InputSystem_Actions();
        playerRigit = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();


        inputAction.Player.Attack.started += OnAttack;
        inputAction.Player.Attack2.started += OnAttack2;

        inputAction.Enable();
    }

    void Update()
    {
        clipInfo = playerAnim.GetCurrentAnimatorClipInfo(0);
        Debug.Log(clipInfo[0].clip.name);
        if (clipInfo[0].clip.name.Contains("Place"))
        {
            playerController.Velocity = Vector3.zero;
        }
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        playerAnim.SetTrigger("isAttack");
        playerAnim.SetInteger("attackType", 0);
    }

    void OnAttack2(InputAction.CallbackContext context)
    {
        playerAnim.SetTrigger("isAttack");
        playerAnim.SetInteger("attackType", 1);
    }



    private void OnDestroy()
    {
        inputAction?.Dispose();
    }
}
