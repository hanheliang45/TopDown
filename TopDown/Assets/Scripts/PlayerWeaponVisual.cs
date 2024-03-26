using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class PlayerWeaponVisual : MonoBehaviour
{
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Animator _animator;
    [SerializeField] private RigController rigController;


    private Dictionary<GunType, Transform> _gunDic;
    private Dictionary<GunType, int> _gunAnimationLayerDic;
    private GunType _selectedGunType;
    
    private void Awake()
    {
        _gunDic = new Dictionary<GunType, Transform>();
        _gunDic.Add(GunType.PISTOL, pistol);
        _gunDic.Add(GunType.REVOLVER, revolver);
        _gunDic.Add(GunType.AUTORIFLE, autoRifle);
        _gunDic.Add(GunType.SHOTGUN, shotgun);
        _gunDic.Add(GunType.RIFLE, rifle);

        _gunAnimationLayerDic = new Dictionary<GunType, int>();
        _gunAnimationLayerDic.Add(GunType.PISTOL, 1);
        _gunAnimationLayerDic.Add(GunType.REVOLVER, 1);
        _gunAnimationLayerDic.Add(GunType.AUTORIFLE, 1);
        _gunAnimationLayerDic.Add(GunType.SHOTGUN, 2);
        _gunAnimationLayerDic.Add(GunType.RIFLE, 3);

    }

    private void Start()
    {
        SwitchOffGuns(GunType.PISTOL);
    }

    private void Update()
    {
        if (PlayerCore.Instance.GetBusy()) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerCore.Instance.SetBusy(true);
            SwitchOffGuns(GunType.PISTOL);
            SwitchOffGunsAnimation(GrabType.SIDE);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerCore.Instance.SetBusy(true);
            SwitchOffGuns(GunType.REVOLVER);
            SwitchOffGunsAnimation(GrabType.SIDE);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerCore.Instance.SetBusy(true);
            SwitchOffGuns(GunType.AUTORIFLE);
            SwitchOffGunsAnimation(GrabType.BEHIND);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerCore.Instance.SetBusy(true);
            SwitchOffGuns(GunType.SHOTGUN);
            SwitchOffGunsAnimation(GrabType.SIDE);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayerCore.Instance.SetBusy(true);
            SwitchOffGuns(GunType.RIFLE);
            SwitchOffGunsAnimation(GrabType.BEHIND);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerCore.Instance.SetBusy(true);
            ReloadAnimation();
        }
    }

    private void ReloadAnimation()
    {
        _animator.SetTrigger("Reload");

        rigController.Deprioritize();
    }

    private void SwitchAnimationLayer(int layer)
    {
        for (int i = 1; i < _animator.layerCount; i++)
        {
            _animator.SetLayerWeight(i, 0);
        }

        _animator.SetLayerWeight(layer, 1);
    }

    private void SwitchOffGuns(GunType gunType)
    {
        _selectedGunType = gunType;

        foreach (var entry in _gunDic)
        {
            entry.Value.gameObject.SetActive(entry.Key == gunType);
        }

        Transform leftHandIKTransform = _gunDic[gunType].transform.Find("IK_target_transform").transform;
        leftHandIKTarget.localPosition = leftHandIKTransform.localPosition;
        leftHandIKTarget.localRotation = leftHandIKTransform.localRotation;

        SwitchAnimationLayer(_gunAnimationLayerDic[gunType]);
    }

    private void SwitchOffGunsAnimation(GrabType grabType)
    {
        _animator.SetTrigger("GrabWeapon");
        _animator.SetFloat("GrabWeaponType", (float)grabType);
        rigController.DeprioritizeLeftHandIK();
    }

    public enum GunType
    {
        PISTOL,
        REVOLVER,
        AUTORIFLE,
        SHOTGUN,
        RIFLE,
    }

    public enum GrabType
    {
        SIDE,
        BEHIND
    }
}
