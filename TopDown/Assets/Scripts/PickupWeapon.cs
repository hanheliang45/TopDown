using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : Interactable
{
    [SerializeField] private Weapon weapon;

    private PlayerWeaponController pwc;
    
    public override void Interact(PlayerInteraction interaction)
    {
        if (pwc.AddWeapon(weapon))
        {
            Destroy(this.transform.parent.gameObject);
            
            interaction.RemoveInteractable(this);
        }
    }
    
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (pwc == null)
        {
            pwc = other.GetComponent<PlayerWeaponController>();
        }
    }

    public Weapon GetWeapon() => weapon;

    //
    // private void OnTriggerExit(Collider other)
    // {
    //     PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
    //     if (interaction != null)
    //     {
    //         interaction.RemoveInteractable(this);
    //     }
    // }
}
