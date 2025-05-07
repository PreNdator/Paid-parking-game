
using UnityEngine;
using Zenject;

public class InputModeManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CursorManager _cursor;

    [Inject]
    private void Construct(PlayerInput playerInput, CursorManager cursor)
    {
        _playerInput = playerInput;
        _cursor = cursor;
    }

    private void Start()
    {
        _playerInput.Always.Enable();
        _playerInput.Always.SwitchMenu.performed += OnSwitchMenu;
    }

    private void OnDestroy()
    {
        _playerInput.Always.SwitchMenu.performed -= OnSwitchMenu;
    }

    private void OnSwitchMenu(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        Toggle();
    }

    public void SwitchToGameplay()
    {
        _playerInput.UI.Disable();
        _playerInput.Player.Enable();
        _cursor.Lock();
    }

    public void SwitchToUI()
    {
        _playerInput.Player.Disable();
        _playerInput.UI.Enable();
        _cursor.Unlock();
    }

    public void Toggle()
    {
        if (_playerInput.Player.enabled)
            SwitchToUI();
        else
            SwitchToGameplay();
    }
}