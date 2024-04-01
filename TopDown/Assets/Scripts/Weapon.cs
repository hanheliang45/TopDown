using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int ammo;
    public int ammoMax;
    public int magazineSize;
    public float fireRate = 1;
    public ShootType shootType;
    public float minSpread = 0;
    public float maxSpread = 10;
    public float increaseSpread = 0.3f;
    public float spreadCoolDown = 2;

    private float lastShootTime;
    private float currentSpread;

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        if (Time.time > lastShootTime + spreadCoolDown)
        {
            currentSpread = minSpread;
            Debug.Log("spreadCoolDown");
        }
        
        Quaternion spreadDirection = Quaternion.Euler(
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread, currentSpread)
            );
        
        currentSpread += increaseSpread;

        return spreadDirection * originalDirection;
    }

    public void Shoot()
    {
        ammo--;

        this.lastShootTime = Time.time;
    }

    public bool HasBullet()
    {
        return ammo > 0;
    }

    public bool IsCoolDown()
    {
        return Time.time > lastShootTime + 1 / fireRate;
    }
}

public enum WeaponType
{
    PISTOL,
    REVOLVER,
    RIFLE,
    SHOTGUN,
    SNIPPER
}

public enum GrabType
{
    SIDE,
    BEHIND
}

public enum ShootType
{
    SINGLE,
    AUTO
}