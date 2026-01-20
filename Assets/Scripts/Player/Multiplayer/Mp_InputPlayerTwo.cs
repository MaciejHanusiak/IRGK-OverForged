using System;
using UnityEngine;

public class Mp_InputPlayerTwo : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnToggleDebug;
    public event EventHandler OnReturn;

    private PlayerInputAction playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputAction();
        playerInputActions.Mp_P2.Enable();

        playerInputActions.Mp_P2.Interact.performed += Interact_performed;
        playerInputActions.Mp_P2.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Mp_P2.ToggleDebug.performed += Toggle_Debug_performed;
        playerInputActions.Mp_P2.Return.performed += Return_performed;

    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void Toggle_Debug_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnToggleDebug?.Invoke(this, EventArgs.Empty);
    }

    private void Return_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnReturn?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;

    }
}
