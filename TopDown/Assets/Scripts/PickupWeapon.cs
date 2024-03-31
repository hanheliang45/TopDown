using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerWeaponController pwc = other.GetComponent<PlayerWeaponController>();
        if (pwc != null)
        {
            if (pwc.Pickup(weapon))
            {
                Destroy(this.gameObject);
            }
        }
        
    }
}
