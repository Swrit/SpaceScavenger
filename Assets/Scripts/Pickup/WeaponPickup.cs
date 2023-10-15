using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup, I_ObjectReset
{
    public override void PickupEffect(ShipControllerPlayer shipControllerPlayer)
    {
        shipControllerPlayer.ProcessWeaponPickup(GetComponent<WeaponBase>());

        base.PickupEffect(shipControllerPlayer);

        enabled = false;
    }

    public override void ResetObject()
    {
        enabled = false;
    }
}
