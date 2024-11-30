using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public PlayerControls inputActions;

    [SerializeField]
    private string controlScheme;

    public Vector2 moveInput;
    public float attackInput;

    public void Awake()
    {
        moveInput = Vector2.zero;
        attackInput = 0;
    }

    public Vector2 GetMovementInput()
    {
        return moveInput;
    }

    public float GetAttackInput()
    {
        return attackInput;
    }

    public void ResetAttackInput()
    {
       // attackInput = 0;
    }

    public void AssignControlScheme(string scheme)
    {
        controlScheme = scheme;

        if(inputActions == null)
        {
            inputActions = new PlayerControls();
            Debug.Log(controlScheme);
            inputActions.bindingMask = new InputBinding { groups = controlScheme };

            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

            inputActions.Player.Attack.performed += ctx => attackInput = ctx.ReadValue<float>();
            inputActions.Player.Attack.canceled += ctx => attackInput = 0;

            inputActions.Player.Enable();
        }
    }

    public void OnDestroy()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;

        inputActions.Player.Attack.performed -= ctx => attackInput = ctx.ReadValue<byte>();
        inputActions.Player.Attack.canceled -= ctx => attackInput = 0;

        inputActions.Player.Disable();
    }
}