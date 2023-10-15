using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private string weaponName;
    [SerializeField] private bool salvageable = false;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] private Pickup pickup;
    protected EntityType ownerType;
    private ShipBehaviour ownerShip;

    private void Start()
    {
        Grabbable grabbable;
        if (TryGetComponent<Grabbable>(out grabbable))
        {
            grabbable.OnGrabbed += Grabbable_OnGrabbed;
        }
    }

    private void Grabbable_OnGrabbed(object sender, System.EventArgs e)
    {
        if (ownerShip == null) return;

        if (pickup != null) pickup.enabled = true;
        ownerShip.DropWeapon(this);
        ownerShip = null;
    }

    public void WeaponSetup(Vector3 direction, EntityType entityType, ShipBehaviour shipBehaviour)
    {
        transform.up = direction;
        ownerType = entityType;
        this.ownerShip = shipBehaviour;
    }
    public abstract void WeaponStart();

    public abstract void WeaponStop();

    public bool IsSalvageable()
    {
        return salvageable;
    }

}
