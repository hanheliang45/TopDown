using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public PlayerController PC;
    
    void Awake()
    {
        PC = new PlayerController();
    }

    private void OnEnable()
    {
        PC.Enable();
    }

    private void OnDisable()
    {
        PC.Disable();
    }
    
}
