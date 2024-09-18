using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public PlayerControls inputActions;

    private PlayerController _playerClass;
    private Vector2 moveInput;

    [SerializeField]
    private string controlScheme;

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
            inputActions.Player.Enable();
        }
    }

    private void Awake()
    {
    }

    private void Start()
    {
        _playerClass = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(_playerClass == null)
        {
            _playerClass = GetComponent<PlayerController>();
        }

        _playerClass.Move(moveInput);
    }


    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Disable();
    }
}