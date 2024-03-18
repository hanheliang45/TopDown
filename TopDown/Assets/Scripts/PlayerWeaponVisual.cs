using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerWeaponVisual : MonoBehaviour
{
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    private Dictionary<GunType, Transform> gunDic;
    void Awake()
    {
        gunDic = new Dictionary<GunType, Transform>();
        gunDic.Add(GunType.PISTOL, pistol);
        gunDic.Add(GunType.REVOLVER, revolver);
        gunDic.Add(GunType.AUTORIFLE, autoRifle);
        gunDic.Add(GunType.SHOTGUN, shotgun);
        gunDic.Add(GunType.RIFLE, rifle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOffGuns(GunType.PISTOL);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOffGuns(GunType.REVOLVER);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOffGuns(GunType.AUTORIFLE);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOffGuns(GunType.SHOTGUN);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOffGuns(GunType.RIFLE);
        }
    }

    private void SwitchOffGuns(GunType gunType)
    {
        foreach (var entry in gunDic)
        {
            entry.Value.gameObject.SetActive(entry.Key == gunType);   
        }
    }

    public enum GunType
    {
        PISTOL,
        REVOLVER,
        AUTORIFLE,
        SHOTGUN,
        RIFLE,
    }
}
