using System;
using UnityEngine;

public class InputProcessor : MonoBehaviour
{
    public event Action<Vector2> OnMoveInput; // x for steering, y for accel
    public event Action<bool> OnHandBrakePressed; // (space by default)
    private VehicleInputActions _actions;

    private void Awake()
    {
        _actions = new VehicleInputActions();
    }

    private void OnEnable()
    {
        _actions.Enable();

        _actions.Drive.Move.performed += OnMovePerformed;
        _actions.Drive.Move.canceled += OnMoveCanceled;
        _actions.Drive.HandBrake.performed += OnHandBrakePerformed;
        _actions.Drive.HandBrake.canceled += OnHandBrakeCanceled;
    }

    private void OnDisable()
    {
        _actions.Drive.Move.performed -= OnMovePerformed;
        _actions.Drive.Move.canceled -= OnMoveCanceled;
        _actions.Drive.HandBrake.performed -= OnHandBrakePerformed;
        _actions.Drive.HandBrake.canceled -= OnHandBrakeCanceled;

        _actions.Disable();
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        OnMoveInput?.Invoke(Vector2.zero);
    }
    private void OnHandBrakePerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        bool isPressed = ctx.ReadValue<float>() > 0.5f;
        OnHandBrakePressed?.Invoke(isPressed);
    }

    private void OnHandBrakeCanceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        OnHandBrakePressed?.Invoke(false);
    }
}

