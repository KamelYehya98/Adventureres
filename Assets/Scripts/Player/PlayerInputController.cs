using Assets.Scripts.Classes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public PlayerControls inputActions;

    private PlayerClassManager _playerClassManager;
    private PlayerClass _playerClass;
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
            inputActions.Player.SwitchClassRight.performed += ctx => OnSwitchClass(1);
            inputActions.Player.SwitchClassLeft.performed += ctx => OnSwitchClass(-1);
            inputActions.Player.Attack.performed += ctx => OnAttack();
            inputActions.Player.SpecialAbility.performed += ctx => OnSpecialAbility();
            inputActions.Player.Enable();
        }
    }

    private void Awake()
    {
    
    }

    private void Start()
    {
        _playerClassManager = GetComponent<PlayerClassManager>();
        _playerClass = GetComponent<PlayerClass>();
    }

    private void Update()
    {
        if(_playerClass == null)
        {
            _playerClass = GetComponent<PlayerClass>();
        }

        if (moveInput != Vector2.zero)
        {
            _playerClass.Move(moveInput);
        }
    }

    private void OnSwitchClass(int direction)
    {
        _playerClassManager.SwitchClass(direction);
    }

    private void OnAttack()
    {
        _playerClass.Attack();
    }

    private void OnSpecialAbility()
    {
        _playerClass.SpecialAbility();
    }

    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.SwitchClassRight.performed -= ctx => OnSwitchClass(1);
        inputActions.Player.SwitchClassLeft.performed -= ctx => OnSwitchClass(-1);
        inputActions.Player.Attack.performed -= ctx => OnAttack();
        inputActions.Player.SpecialAbility.performed -= ctx => OnSpecialAbility();
        inputActions.Player.Disable();
    }
}
