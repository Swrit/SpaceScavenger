using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerControls playerControls;

    public event EventHandler OnAction1;
    public event EventHandler OnAction1Cancel;
    public event EventHandler OnAction2;
    public event EventHandler OnAction2Cancel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        playerControls = new PlayerControls();
        playerControls.PlayerShip.Enable();
        playerControls.PlayerShip.Action1.performed += Action1_performed;
        playerControls.PlayerShip.Action1.canceled += Action1_canceled1;
        playerControls.PlayerShip.Action2.performed += Action2_performed;
        playerControls.PlayerShip.Action2.canceled += Action2_canceled;
    }

    public Vector2 GetMoveVector()
    {
        Vector2 inputVector = playerControls.PlayerShip.Arrows.ReadValue<Vector2>();

        return inputVector;
    }

    private void Action1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAction1?.Invoke(this, EventArgs.Empty);
    }
    
    private void Action1_canceled1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAction1Cancel?.Invoke(this, EventArgs.Empty);
    }

    private void Action2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAction2?.Invoke(this, EventArgs.Empty);
    }

    private void Action2_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAction2Cancel?.Invoke(this, EventArgs.Empty);
    }


}
