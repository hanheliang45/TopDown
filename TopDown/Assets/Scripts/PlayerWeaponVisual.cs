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

    [SerializeField] private Transform leftHandIK;
    
    private Dictionary<GunType, Transform> _gunDic;
    private GunType _selectedGunType;
    void Awake()
    {
        _gunDic = new Dictionary<GunType, Transform>();
        _gunDic.Add(GunType.PISTOL, pistol);
        _gunDic.Add(GunType.REVOLVER, revolver);
        _gunDic.Add(GunType.AUTORIFLE, autoRifle);
        _gunDic.Add(GunType.SHOTGUN, shotgun);
        _gunDic.Add(GunType.RIFLE, rifle);
    }

    private void Start()
    {
        SwitchOffGuns(GunType.PISTOL);
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
        _selectedGunType = gunType;
        
        foreach (var entry in _gunDic)
        {
            entry.Value.gameObject.SetActive(entry.Key == gunType);   
        }

        Transform leftHandIKTarget = _gunDic[gunType].transform.Find("IK_target_transform").transform;
        leftHandIK.localPosition = leftHandIKTarget.localPosition;
        leftHandIK.localRotation = leftHandIKTarget.localRotation;
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
