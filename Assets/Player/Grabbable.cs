using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public event EventHandler OnGrabbed;
    public event EventHandler OnUngrabbed;

    [SerializeField] private float weight = 1f;
    public void Grab()
    {
        OnGrabbed?.Invoke(this, EventArgs.Empty);
    }

    public void Ungrab()
    {
        OnUngrabbed?.Invoke(this, EventArgs.Empty);
    }

    public float GetWeight()
    {
        return weight;
    }

    public void PickUp(ShipControllerPlayer shipControllerPlayer)
    {
        GetComponent<Pickup>().PickupEffect(shipControllerPlayer);
    }


}
