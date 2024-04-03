using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class PlayerWeaponVisual : MonoBehaviour
{
    // [SerializeField] private Transform pistol;
    // [SerializeField] private Transform revolver;
    // [SerializeField] private Transform autoRifle;
    // [SerializeField] private Transform shotgun;
    // [SerializeField] private Transform rifle;

    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Animator _animator;
    [SerializeField] private RigController rigController;


    // private Dictionary<WeaponType, Transform> _gunDic;
    // private Dictionary<WeaponType, int> _gunAnimationLayerDic;

    private Dictionary<WeaponType, WeaponModel> _gun2ModelDic;
    // private WeaponType _selectedGunType;
    
    private void Awake()
    {
        _gun2ModelDic = new Dictionary<WeaponType, WeaponModel>();

        WeaponModel[] weaponModels = this.GetComponentsInChildren<WeaponModel>(true);
        foreach (WeaponModel model in weaponModels)
        {
            _gun2ModelDic.Add(model.GetWeaponType(), model);
        }
    }

    public Transform GetFirePoint(WeaponType weaponType)
    {
        return _gun2ModelDic[weaponType].transform.Find("FirePoint");
    }

    public void ReloadAnimation(WeaponType weaponType)
    {
        _animator.SetTrigger("Reload");
        _animator.SetFloat("ReloadSpeed", _gun2ModelDic[weaponType].GetReloadSpeed());

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

    public void SwitchWeaponModel(WeaponType weaponType)
    {
        // _selectedGunType = weaponType;

        foreach (var entry in _gun2ModelDic)
        {
            entry.Value.gameObject.SetActive(entry.Key == weaponType);
        }

        Transform leftHandIKTransform = _gun2ModelDic[weaponType].transform.Find("IK_target_transform").transform;
        leftHandIKTarget.localPosition = leftHandIKTransform.localPosition;
        leftHandIKTarget.localRotation = leftHandIKTransform.localRotation;

        SwitchAnimationLayer(_gun2ModelDic[weaponType].GetAnimationLayer());
    }

    public void GrabGunAnimation(WeaponType weaponType)
    {
        _animator.SetTrigger("GrabWeapon");
        _animator.SetFloat("GrabWeaponType", (float)_gun2ModelDic[weaponType].GetGrabType());
        rigController.DeprioritizeLeftHandIK();
    }
    
}
