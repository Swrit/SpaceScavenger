using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour, I_ObjectReset
{
    private I_ShipControllerInterface shipController;

    [SerializeField] private EntityType entityType = EntityType.enemy;

    [SerializeField] private BoxCollider2D clampMovementBox;
    private Bounds clampMovement;

    [SerializeField] private List<WeaponSlot> weaponSlots;

    [Serializable]
    private struct WeaponSlot
    {
        public Transform mountPoint;
        public WeaponBase weapon;
        public bool permanent;
    }

    [SerializeField] private bool getArmed = true;
    [SerializeField] private List<WeaponBase> weaponPool;

    [SerializeField] protected bool faceUp = false;
    protected Vector3 faceDirection = Vector3.down;

    [SerializeField] private float strafeSpeed = 256f;

    [SerializeField] private int pointsForKill = 0;

    protected Health health;

    [SerializeField] private AudioClip deathSound;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        if (faceUp) faceDirection = Vector3.up;

        transform.up = faceDirection;
    }




    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (shipController == null) shipController = GetComponent<I_ShipControllerInterface>();

        shipController.OnWeaponStart += ShipController_OnWeaponStart;
        shipController.OnWeaponStop += ShipController_OnWeaponStop;

        health = GetComponent<Health>();
        health.OnHealthChange += Health_OnHealthChange;

        ResetObject();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = shipController.GetMovementVector() * strafeSpeed * Time.deltaTime;

        Vector3 newPosition = transform.position + new Vector3(moveInput.x, moveInput.y, 0f);

        if (clampMovementBox != null)
        {
            clampMovement = clampMovementBox.bounds;
            newPosition.x = Mathf.Clamp(newPosition.x, clampMovement.min.x, clampMovement.max.x);
            newPosition.y = Mathf.Clamp(newPosition.y, clampMovement.min.y, clampMovement.max.y);
        }
        

        transform.position = newPosition;

        if (PlayArea.Instance.IsOutsideActiveZone(boxCollider))
        {
            Debug.Log("Dispose outside active zone");
            Dispose();
        }
    }

    private void ShipController_OnWeaponStart(object sender, int e)
    {
        if (e >= weaponSlots.Count) return;
        
        weaponSlots[e].weapon?.WeaponStart();
    }

    private void ShipController_OnWeaponStop(object sender, int e)
    {
        if (e > weaponSlots.Count) return;

        weaponSlots[e].weapon?.WeaponStop();
    }

    private void Health_OnHealthChange(object sender, Vector2Int e)
    {
        if (e.x == 0) Die();
    }

    public void ResetObject()
    {
        GameObject newWeapon = null;
        if (getArmed)
        {
            int selectWeapon = UnityEngine.Random.Range(0, weaponPool.Count);
            newWeapon = weaponPool[selectWeapon].gameObject;
        }

        for (int i = 0; i < weaponSlots.Count; i++)
        {
            Debug.Log("Slot " + i + " weapon " + newWeapon);
            if (weaponSlots[i].permanent) 
            {
                weaponSlots[i].weapon.WeaponSetup(transform.up, entityType, this);
                continue;
            };
            GameObject weapon = ObjectPoolManager.Instance.RequestObject(newWeapon);
            if (weapon)
            {
                GetWeapon(weapon.GetComponent<WeaponBase>(), i);
            }
            
            
        }

    }

    public int HowManyWeapons()
    {
        return weaponSlots.Count;
    }

    public void RemoveWeapon(int slotNumber)
    {
        if (weaponSlots[slotNumber].weapon == null) return;
        ObjectPoolManager.Instance.Deactivate(weaponSlots[slotNumber].weapon.gameObject);
        SetSlotWeapon(null, slotNumber);
    }

    public void DropWeapon(WeaponBase weapon)
    {
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (weaponSlots[i].weapon == weapon)
            {
                SetSlotWeapon(null, i);
            }
        }
        
    }

    public void GetWeapon(WeaponBase weapon, int slotNumber)
    {
        RemoveWeapon(slotNumber);
        SetSlotWeapon(weapon, slotNumber);
        weapon.transform.position = weaponSlots[slotNumber].mountPoint.transform.position;
        weapon.transform.SetParent(weaponSlots[slotNumber].mountPoint.transform, true);
        weapon.WeaponSetup(transform.up, entityType, this);
    }

    private void SetSlotWeapon(WeaponBase weapon, int slotNumber)
    {
        WeaponSlot modSlot = weaponSlots[slotNumber];
        modSlot.weapon = weapon;
        weaponSlots[slotNumber] = modSlot;
    }

    public void SetClampBox(BoxCollider2D box)
    {
        clampMovementBox = box;
    }

    protected void Dispose()
    {
        //Debug.Log(gameObject.GetInstanceID() + " dispose");
        ObjectPoolManager.Instance.Deactivate(gameObject);
    }

    private void Die()
    {
        SoundManager.Instance.PlaySound(deathSound);
        GuiController.Instance.AddScore(pointsForKill);
        Dispose();
    }
}
