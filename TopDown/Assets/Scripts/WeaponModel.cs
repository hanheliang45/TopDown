using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponModel : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private int animationLayer;
    [SerializeField] private GrabType grabType;
    [Range(0.5f, 3)]
    [SerializeField] private float reloadSpeed;

    public WeaponType GetWeaponType() => weaponType;
    public int GetAnimationLayer() => animationLayer;
    public GrabType GetGrabType() => grabType;
    public float GetReloadSpeed() => reloadSpeed;


}
