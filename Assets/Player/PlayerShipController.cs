using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public event EventHandler OnAction1;
    public event EventHandler OnAction1Cancel;


    [SerializeField] private float strafeSpeed = 5f;


    private void Start()
    {
        InputManager.Instance.OnAction1 += InputManager_OnAction1;
        InputManager.Instance.OnAction1Cancel += InputManager_OnAction1Cancel;
        InputManager.Instance.OnAction2 += InputManager_OnAction2;
        InputManager.Instance.OnAction2Cancel += InputManager_OnAction2Cancel;
    }

    private void Update()
    {
        Vector2 moveInput = InputManager.Instance.GetMoveVector();

        if (moveInput.x != 0) transform.position += new Vector3 (moveInput.x * strafeSpeed * Time.deltaTime, 0, 0);
    }


    private void InputManager_OnAction1(object sender, System.EventArgs e)
    {
        OnAction1?.Invoke(this, EventArgs.Empty);
    }
    private void InputManager_OnAction1Cancel(object sender, System.EventArgs e)
    {
        OnAction1Cancel?.Invoke(this, EventArgs.Empty);
    }

    private void InputManager_OnAction2(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    } 
    private void InputManager_OnAction2Cancel(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }
}
