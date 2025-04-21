using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;


[RequireComponent(typeof(PlayerMovement))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _controls;
    private PlayerMovement _movement;
    private InputModeManager _inputModeManager;

    [Inject]
    private void Construct(PlayerInput controls, InputModeManager inputModeManager)
    {
        _controls = controls;
        _inputModeManager = inputModeManager;
    }

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _inputModeManager.SwitchToGameplay();

        _controls.Player.Jump.performed += OnJump;
        _controls.Player.Sprint.started += OnSprintStarted;
        _controls.Player.Sprint.canceled += OnSprintCanceled;
        _controls.Player.Aim.performed += OnAim;
        _controls.Player.TakePhoto.performed += OnTakePhoto;
    }

    private void OnDisable()
    {
        _controls.Player.Jump.performed -= OnJump;
        _controls.Player.Sprint.started -= OnSprintStarted;
        _controls.Player.Sprint.canceled -= OnSprintCanceled;
        _controls.Player.Aim.performed -= OnAim;
        _controls.Player.TakePhoto.performed -= OnTakePhoto;
    }

    private void Update()
    {
        Vector2 moveInput = _controls.Player.Move.ReadValue<Vector2>();

        _movement.Move(moveInput);
    }

    void LateUpdate()
    {
        Vector2 look = _controls.Player.Look.ReadValue<Vector2>();
        _movement.Look(look);
    }

    private void OnJump(InputAction.CallbackContext ctx) => _movement.Jump();
    private void OnSprintStarted(InputAction.CallbackContext ctx) => _movement.SetSprinting(true);
    private void OnSprintCanceled(InputAction.CallbackContext ctx) => _movement.SetSprinting(false);
    private void OnAim(InputAction.CallbackContext ctx) => Aim();
    private void OnTakePhoto(InputAction.CallbackContext ctx) => TakePhoto();

    private void Aim()
    {
        Debug.Log("Aim triggered");
    }

    private void TakePhoto()
    {
        Debug.Log("Photo taken!");
    }
}