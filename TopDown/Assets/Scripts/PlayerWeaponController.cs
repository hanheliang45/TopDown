using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    private PlayerController _controller;
    private PlayerWeaponVisual _weaponVisual;
    private Animator _animator;
    private bool isShootPressed;

    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private float bulletSpeed;
    // [SerializeField] private Transform gunFirePoint;
    [SerializeField] private Transform firstPickupGun;
    [SerializeField] private List<Weapon> weaponSlots;
    private Weapon currentWeapon;
    
    private void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
        _weaponVisual = this.GetComponent<PlayerWeaponVisual>();
        
    }

    private void Start()
    {
        _controller = GetComponent<PlayerCore>().PC;
        
        _controller.Character.Fire.performed += context => isShootPressed = true;
        _controller.Character.Fire.canceled += context => isShootPressed = false;
        
        _controller.Character.Equip1.performed += context => EquipWeapon(0);
        _controller.Character.Equip2.performed += context => EquipWeapon(1);

        _controller.Character.Interact.performed += context => Pickup();
        
        _controller.Character.Drop.performed += context => DropCurrentWeapon();
        _controller.Character.Reload.performed += context => Reload();

        AddWeapon(firstPickupGun.GetComponentInChildren<PickupWeapon>().GetWeapon());
        EquipWeapon(0);
    }

    private void Update()
    {
        if (isShootPressed)
        {
            if (Shoot())
            {
                if (currentWeapon.shootType == ShootType.SINGLE)
                {
                    isShootPressed = false;
                }
            }
        }
    }

    public void DropCurrentWeapon()
    {
        if (weaponSlots.Count <= 1)
        {
            return;
        }

        weaponSlots.Remove(currentWeapon);
        Instantiate(
            PickupManager.Instance.GetWeaponPrefab(currentWeapon.weaponType),
            this.transform.position + Vector3.up,
            Quaternion.identity
        );

        currentWeapon = weaponSlots[0];

        EquipWeapon(0);
    }

    public void EquipWeapon(int slotIndex)
    {
        if (slotIndex >= weaponSlots.Count)
        {
            return;
        }
        if (PlayerCore.Instance.GetBusy()) return;
        
        PlayerCore.Instance.SetBusy(true);

        currentWeapon = weaponSlots[slotIndex];
        currentWeapon.Equip();
        
        _weaponVisual.SwitchWeaponModel(currentWeapon.weaponType);
        _weaponVisual.GrabGunAnimation(currentWeapon.weaponType);
    }

    public void Pickup()
    {
        PlayerCore.Instance.GetPlayerInteraction().Interact();
    }

    public bool AddWeapon(Weapon weapon)
    {
        if (weaponSlots.Count < 2)
        {
            weaponSlots.Add(weapon);
            return true;
        }

        return false;
    }

    private void Reload()
    {
        if (PlayerCore.Instance.GetBusy()) return;
        
        if (currentWeapon.ammoMax <= 0)
        {
            return;
        }

        int toBeAdd = currentWeapon.magazineSize - currentWeapon.ammo;
        if (toBeAdd <= 0)
        {
            return;
        }
        PlayerCore.Instance.SetBusy(true);

        toBeAdd = Mathf.Min(toBeAdd, currentWeapon.ammoMax);
        currentWeapon.ammo += toBeAdd;
        currentWeapon.ammoMax -= toBeAdd;

        _weaponVisual.ReloadAnimation(currentWeapon.weaponType);
    }

    private bool Shoot()
    {
        if (PlayerCore.Instance.GetBusy()) return false;

        if (currentWeapon == null)
        {
            return false;
        }

        if (!this.currentWeapon.HasBullet()
            || !this.currentWeapon.IsCoolDown())
        {
            return false;
        }
        
        ShootBullet();

        return true;
    }

    private void ShootBullet()
    {
        Transform gunFirePoint = this._weaponVisual.GetFirePoint(currentWeapon.weaponType);
        Transform newBullet = Instantiate(bulletPrefab, gunFirePoint.position, Quaternion.LookRotation(gunFirePoint.forward));

        newBullet.GetComponent<Rigidbody>().velocity = 
            currentWeapon.ApplySpread(gunFirePoint.forward) * bulletSpeed;
        
        Destroy(newBullet.gameObject, 10);

        this.currentWeapon.Shoot();
        
        _animator.SetTrigger("Fire");
    }

    public Transform GetGunPoint() => this._weaponVisual.GetFirePoint(currentWeapon.weaponType);
}
