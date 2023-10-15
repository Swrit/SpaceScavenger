using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_ShipControllerInterface
{
    public event EventHandler<int> OnWeaponStart;
    public event EventHandler<int> OnWeaponStop;

    public Vector2 GetMovementVector();


}
