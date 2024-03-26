using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{

    public static PlayerCore Instance;
    
    public PlayerController PC;
    private bool _busy;

    private PlayerAim _playerAim;
    private PlayerMovement _playerMovement;
    private PlayerWeaponController _weaponController;
    
    void Awake()
    {
        PC = new PlayerController();
        Instance = this;

        _playerAim = GetComponent<PlayerAim>();
        _playerMovement = GetComponent<PlayerMovement>();
        _weaponController = GetComponent<PlayerWeaponController>();
    }

    public void SetBusy(bool busyOrNot)
    {
        this._busy = busyOrNot;
    }

    public PlayerAim GetPlayerAim() => _playerAim;
    public PlayerMovement GetPlayerMovement() => _playerMovement;

    public PlayerWeaponController GetWeaponController() => _weaponController;

    public bool GetBusy() => _busy;

    private void OnEnable()
    {
        PC.Enable();
    }

    private void OnDisable()
    {
        PC.Disable();
    }
    
}
