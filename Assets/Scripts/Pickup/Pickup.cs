using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, I_ObjectReset
{
    [SerializeField] private bool disposeOnPickup = true;
    [SerializeField] private float moveSpeed;
    [SerializeField] private AudioClip pickupSound;

    private BoxCollider2D boxCollider;
    private Vector3 moveDirection = Vector3.down;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        ResetObject();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = moveSpeed * Time.deltaTime;

        RaycastHit2D[] raycasts = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f, moveDirection, distance);
        if (raycasts.Length > 0)
        {
            foreach (RaycastHit2D raycastHit2D in raycasts)
            {
                ShipControllerPlayer playerShip = raycastHit2D.transform.GetComponent<ShipControllerPlayer>();
                if (playerShip == null) continue;
                Debug.Log("PU dispose");
                PickupEffect(playerShip);
                
            }
        }

        transform.position += moveDirection * distance;

        if (PlayArea.Instance.IsOutsideActiveZone(boxCollider))
        {
            Debug.Log("Dispose outside active zone");
            Dispose();
        }
    }

    public virtual void PickupEffect(ShipControllerPlayer shipControllerPlayer) 
    {
        SoundManager.Instance.PlaySound(pickupSound);
        if (disposeOnPickup) Dispose();
    }

    protected void Dispose()
    {
        //Debug.Log(gameObject.GetInstanceID() + " dispose");
        ObjectPoolManager.Instance.Deactivate(gameObject);
    }

    public virtual void ResetObject()
    {
        
    }
}
