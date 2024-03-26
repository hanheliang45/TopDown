using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    private PlayerController _controller;
    private Animator _animator;

    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunFirePoint;
    
    void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _controller = GetComponent<PlayerCore>().PC;
        
        _controller.Character.Fire.performed += context => Shoot();
    }

    private void Reload()
    {
     
    }

    private void Shoot()
    {
        if (PlayerCore.Instance.GetBusy())
        {
            return;
        }

        Transform newBullet = Instantiate(bulletPrefab, gunFirePoint.position, Quaternion.LookRotation(gunFirePoint.forward));

        newBullet.GetComponent<Rigidbody>().velocity = gunFirePoint.forward * bulletSpeed;
        
        Destroy(newBullet.gameObject, 10);
        
        _animator.SetTrigger("Fire");
    }

    public Transform GetGunPoint() => this.gunFirePoint;
}
