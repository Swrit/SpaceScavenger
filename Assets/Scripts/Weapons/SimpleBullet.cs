using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour, I_AttackInterface
{
    private EntityType ownerType;
    [SerializeField] private int damage;

    [SerializeField] private float moveSpeed;

    [SerializeField] private bool disposeAfterHit = true;

    private BoxCollider2D boxCollider;

    public void AttackSetup(Vector3 direction, EntityType ownerType)
    {
        transform.up = direction;
        this.ownerType = ownerType;
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = moveSpeed * Time.deltaTime;

        RaycastHit2D[] raycasts = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f, transform.up, distance);
        if (raycasts.Length > 0)
        {
            foreach (RaycastHit2D raycastHit2D in raycasts)
            {
                Health targetHealth = raycastHit2D.transform.GetComponent<Health>();
                if (targetHealth == null) continue;
                if (!TryHit(targetHealth)) continue;
                if (disposeAfterHit)
                {
                    //Debug.Log("Dispose after hit " + raycastHit2D.transform.gameObject.name);
                    Dispose();
                }
            }
        }

        transform.position += transform.up * distance;

        if (PlayArea.Instance.IsOutsideActiveZone(boxCollider)) 
        {
            //Debug.Log("Dispose outside active zone");
            Dispose(); 
        }
    }

    private bool TryHit(Health targetHealth)
    {
        return targetHealth.TryGetHit(damage, ownerType);
    }

    private void Dispose()
    {
        //Debug.Log(gameObject.GetInstanceID() + " dispose");
        ObjectPoolManager.Instance.Deactivate(gameObject);
    }

}
