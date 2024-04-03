using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    [SerializeField] private Transform pickupPistolPrefab;
    [SerializeField] private Transform pickupRevolverPrefab;
    [SerializeField] private Transform pickupRiflePrefab;
    [SerializeField] private Transform pickupShotgunPrefab;
    [SerializeField] private Transform pickupSnipperPrefab;
    
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
        pickupWeaponDic.Add(WeaponType.RIFLE, pickupRiflePrefab);
        pickupWeaponDic.Add(WeaponType.SHOTGUN, pickupShotgunPrefab);
        pickupWeaponDic.Add(WeaponType.SNIPPER, pickupSnipperPrefab);
    }

    public Transform GetWeaponPrefab(WeaponType wt) => pickupWeaponDic[wt];
}
