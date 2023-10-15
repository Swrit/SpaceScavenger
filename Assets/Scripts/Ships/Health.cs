using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, I_ObjectReset
{
    public event EventHandler<Vector2Int> OnHealthChange;

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    [SerializeField] private List<EntityType> damagedBy = new List<EntityType>();

    private bool vulnerable = true;

    public void ResetObject()
    {
        ChangeHealth(maxHealth);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<ShipControllerPlayer>()) GuiController.Instance.SubscribeHealth(this);

        ResetObject();
    }

    public bool TryGetHit(int damage, EntityType entityType)
    {
        if (damagedBy.Contains(entityType))
        {
            if (vulnerable) ChangeHealth(currentHealth - damage);
            return true;
        }
        return false;
    }

    public void Heal(int heal)
    {
        ChangeHealth(currentHealth + heal);
    }

    private void ChangeHealth(int setHealth)
    {
        int newHealth = Mathf.Clamp(setHealth, 0, maxHealth);
        if (newHealth!=currentHealth)
        {
            currentHealth = newHealth;
            //float ratio = (float)currentHealth / maxHealth;
            OnHealthChange?.Invoke(this, new Vector2Int(currentHealth, maxHealth));
        }

    }

    public void SetVulnerability(bool vulnerability)
    {
        vulnerable = vulnerability;
    }
}
