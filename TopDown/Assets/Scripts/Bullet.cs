using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform bulletImpactPref;
    
    void Start()
    {
    }

    private void OnCollisionEnter(Collision other)
    {
        creatBulletImpact(other);
        
        Destroy(gameObject);
    }

    private void creatBulletImpact(Collision other)
    {
        Transform impact = Instantiate(bulletImpactPref, other.contacts[0].point, 
            Quaternion.LookRotation(other.contacts[0].normal));
        Destroy(impact.gameObject, 1f);
    }
}
