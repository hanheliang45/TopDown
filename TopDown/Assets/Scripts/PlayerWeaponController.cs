using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    private PlayerController _controller;
    private PlayerWeaponVisual _weaponVisual;
    private Animator _animator;

    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunFirePoint;

    [SerializeField] private List<Weapon> weaponSlots;
    private Weapon currentWeapon;
    
    void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
        _weaponVisual = this.GetComponent<PlayerWeaponVisual>();
    }

    void Start()
    {
        _controller = GetComponent<PlayerCore>().PC;
        
        _controller.Character.Fire.performed += context => Shoot();
        _controller.Character.Equip1.performed += context => EquipWeapon(0);
        _controller.Character.Equip2.performed += context => EquipWeapon(1);
        _controller.Character.Drop.performed += context => DropCurrentWeapon();
        _controller.Character.Reload.performed += context => Reload();
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
        // todo grab weapon animation
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
        
        _weaponVisual.SwitchOffGuns(currentWeapon.weaponType);
        _weaponVisual.SwitchOffGunsAnimation(currentWeapon.weaponType);
    }

    public bool Pickup(Weapon weapon)
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

        _weaponVisual.ReloadAnimation();
    }

    private void Shoot()
    {
        if (PlayerCore.Instance.GetBusy()) return;

        if (currentWeapon == null)
        {
            return;
        }

        if (currentWeapon.ammo == 0)
        {
            return;
        }

        currentWeapon.ammo--;
        
        ShootBullet();
    }

    private void ShootBullet()
    {
        Transform newBullet = Instantiate(bulletPrefab, gunFirePoint.position, Quaternion.LookRotation(gunFirePoint.forward));

        newBullet.GetComponent<Rigidbody>().velocity = gunFirePoint.forward * bulletSpeed;
        
        Destroy(newBullet.gameObject, 10);
        
        _animator.SetTrigger("Fire");
    }

    public Transform GetGunPoint() => this.gunFirePoint;
}
