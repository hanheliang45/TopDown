using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    private PlayerController _controller;
    private Animator _animator;
    
    void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _controller = GetComponent<PlayerCore>().PC;
        
        _controller.Character.Fire.performed += context => Shoot();
        //_controller.Character.Reload.performed += context => Reload();
    }

    private void Reload()
    {
     
    }

    private void Shoot()
    {
        _animator.SetTrigger("Fire");
    }
}
