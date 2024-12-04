using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    InputSystem_Actions inputAction;
    Animator playerAnim;

    bool isAttack = false;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        inputAction = new InputSystem_Actions();

        inputAction.Player.Attack.started += OnAttack;
        inputAction.Player.Attack.canceled += OnAttack;

        inputAction.Enable();
    }

    void Update()
    {
        if(isAttack)
        {
            playerAnim.SetBool("isAttack",true);
        }
        else
        {
            playerAnim.SetBool("isAttack", false);
        }
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        isAttack = true;
        if (context.canceled)
        {
            isAttack = false;
        }
    }

    private void OnDestroy()
    {
        inputAction?.Dispose();
    }
}
