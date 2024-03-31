using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    [SerializeField] private Transform pickupPistolPrefab;
    [SerializeField] private Transform pickupRevolverPrefab;
    
    private Dictionary<WeaponType, Transform> pickupWeaponDic;
    
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pickupWeaponDic = new Dictionary<WeaponType, Transform>();
        pickupWeaponDic.Add(WeaponType.PISTOL, pickupPistolPrefab);
        pickupWeaponDic.Add(WeaponType.REVOLVER, pickupRevolverPrefab);
    }

    public Transform GetWeaponPrefab(WeaponType wt) => pickupWeaponDic[wt];
}
