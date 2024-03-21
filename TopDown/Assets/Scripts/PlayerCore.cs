using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{

    public static PlayerCore Instance;
    
    public PlayerController PC;
    private bool _busy;
    
    void Awake()
    {
        PC = new PlayerController();
        Instance = this;
    }

    public void SetBusy(bool busyOrNot)
    {
        this._busy = busyOrNot;
    }

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
