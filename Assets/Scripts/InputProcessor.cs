using System;
using UnityEngine;

//TODO explain
public interface IVehicleInput
{
    event Action<Vector2> OnMoveInput;
    event Action<bool> OnHandBrakePressed;
}
public class InputProcessor : MonoBehaviour, IVehicleInput
{
    public event Action<Vector2> OnMoveInput; // x for steering, y for accel
    public event Action<bool> OnHandBrakePressed; // (space by default)
    private VehicleInputActions _actions;

    //those below will store values to pass into subscribers
    private Vector2 moveInput = Vector2.zero;
    private bool handBrakePressed = false;

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
        moveInput = ctx.ReadValue<Vector2>();
        OnMoveInput?.Invoke(moveInput);
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
        OnMoveInput?.Invoke(moveInput);
    }

    private void OnHandBrakePerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        handBrakePressed = ctx.ReadValue<float>() > 0.5f;
        OnHandBrakePressed?.Invoke(handBrakePressed);
    }

    private void OnHandBrakeCanceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        handBrakePressed = false;
        OnHandBrakePressed?.Invoke(handBrakePressed);
    }
}