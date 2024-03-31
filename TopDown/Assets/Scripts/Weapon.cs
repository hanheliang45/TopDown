using System;
using UnityEngine.Serialization;

[Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int ammo;
    public int ammoMax;
    public int magazineSize;
}

public enum WeaponType
{
    PISTOL,
    REVOLVER,
    RIFLE,
    SHOTGUN,
    SNIPPER
}

public enum GrabType
{
    SIDE,
    BEHIND
}