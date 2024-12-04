using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    InputSystem_Actions inputAction;
    Animator playerAnim;
    Rigidbody playerRigit;
    PlayerController playerController;

    bool isAttack = false;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        inputAction = new InputSystem_Actions();
        playerRigit = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();

        inputAction.Player.Attack.started += OnAttack;
        inputAction.Player.Attack.canceled += OnAttackCancel;

        inputAction.Enable();
    }

    void Update()
    {
        Debug.Log(isAttack);
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        isAttack = true;
        playerAnim.SetTrigger("isAttack");
        
    }

    void OnAttackCancel(InputAction.CallbackContext context)
    {
        isAttack= false;
    }

    private void OnDestroy()
    {
        inputAction?.Dispose();
    }
}
