using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickup : Pickup
{
    [SerializeField] private int healAmount;

    public override void PickupEffect(ShipControllerPlayer shipControllerPlayer)
    {
        shipControllerPlayer.ProcessHealthPickup(healAmount);

        base.PickupEffect(shipControllerPlayer);
    }

}
