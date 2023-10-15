using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControllerPlayer : MonoBehaviour, I_ShipControllerInterface
{
    public event EventHandler<int> OnWeaponStart;
    public event EventHandler<int> OnWeaponStop;

    private int primaryWeaponInt = 0;
    private int secondaryWeaponInt = 1;

    public Vector2 GetMovementVector()
    {
        Vector2 input = InputManager.Instance.GetMoveVector();
        input.y = 0f;
        return input;
    }

    private void Start()
    {
        InputManager.Instance.OnAction1 += InputManager_OnAction1;
        InputManager.Instance.OnAction1Cancel += InputManager_OnAction1Cancel;
        InputManager.Instance.OnAction2 += InputManager_OnAction2;
        InputManager.Instance.OnAction2Cancel += InputManager_OnAction2Cancel;

        GameManager.Instance.SetPlayer(this);
    }

    public void ProcessHealthPickup(int healAmount)
    {
        GetComponent<Health>().Heal(healAmount);
    }

    public void ProcessWeaponPickup(WeaponBase weaponBase)
    {
        GetComponent<ShipBehaviour>().GetWeapon(weaponBase, secondaryWeaponInt);
    }
    private void InputManager_OnAction1(object sender, EventArgs e)
    {
        OnWeaponStart?.Invoke(this, primaryWeaponInt);
    }

    private void InputManager_OnAction1Cancel(object sender, EventArgs e)
    {
        OnWeaponStop?.Invoke(this, primaryWeaponInt);
    }

    private void InputManager_OnAction2(object sender, EventArgs e)
    {
        OnWeaponStart?.Invoke(this, secondaryWeaponInt);
    }

    private void InputManager_OnAction2Cancel(object sender, EventArgs e)
    {
        OnWeaponStop?.Invoke(this, secondaryWeaponInt);
    }
}
