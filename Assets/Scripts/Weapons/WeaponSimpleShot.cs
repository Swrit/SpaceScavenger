using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSimpleShot : WeaponBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shotSound;

    [SerializeField] private bool infiniteBurst = false;
    [SerializeField] private int shotsPerBurst = 1;
    [SerializeField] private float shotCooldown = 0.2f;
    [SerializeField] private float burstCooldown = 0f;
    private float shotTimer = 0f;
    private float burstTimer = 0f;
    private int burstCounter = 0;
    private bool weaponActive = false;

    public override void WeaponStart()
    {
        weaponActive = true;
    }

    public override void WeaponStop()
    {
        shotTimer = 0f;
        burstCounter = 0;
        weaponActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (burstTimer > 0)
        {
            burstTimer = Mathf.Max(0, burstTimer - Time.deltaTime);
            return;
        }

        if (!weaponActive) return;

        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            return;
        }

        Shoot();
        burstCounter++;

        if (burstCounter >= shotsPerBurst)
        {
            burstCounter = 0;
            burstTimer = burstCooldown;
        }
        else
        {
            shotTimer = shotCooldown;
        }
    }

    private void Shoot()
    {
        SoundManager.Instance.PlaySound(shotSound);
        //Debug.Log("Shoot");
        GameObject bullet = ObjectPoolManager.Instance.RequestObjectAt(bulletPrefab, shootPoint.position);
        bullet.GetComponent<I_AttackInterface>().AttackSetup(transform.up, ownerType);
    }

}
