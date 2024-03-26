using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _controller;

    public Vector2 _moveInput { private set; get; }


    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private float _verticalVelocity;

    private Animator _animator;
    private bool _isRunning;
    
    [Header("Movement info")]
    [@SerializeField] private float walkSpeed = 5;
    [@SerializeField] private float runSpeed = 8;
    [@SerializeField] private float turnSpeed = 4;

    
    void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
        _animator = this.GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _controller = GetComponent<PlayerCore>().PC;
        AssignInput();
    }

    private void AssignInput()
    {
        _controller.Character.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
        _controller.Character.Move.canceled += context => _moveInput = Vector2.zero;

        _controller.Character.Run.performed += context => _isRunning = true;
        _controller.Character.Run.canceled += context => _isRunning = false;
    }
    

    void Update()
    {
        ApplyMovement();
        ApplyGravity();
        ApplyRotation();
        
        ApplyMoveAnimator();
    }

    private void ApplyMoveAnimator()
    {
        float xVelocity = Vector3.Dot(_moveDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(_moveDirection.normalized, transform.forward);
        
        _animator.SetFloat("x_velocity", xVelocity, 0.2f, Time.deltaTime);
        _animator.SetFloat("z_velocity", zVelocity, 0.2f,Time.deltaTime);
        _animator.SetBool("isRuning", _isRunning && _moveInput.magnitude > 0);
    }

    private void ApplyRotation()
    {
        Vector3 lookingDirection = PlayerCore.Instance.GetPlayerAim().GetMouseHitInfo().point  - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }


    private void ApplyGravity()
    {
        if (!_characterController.isGrounded)
        {
            _verticalVelocity -= 9.8f * Time.deltaTime;
            _moveDirection.y = _verticalVelocity;
        }
        else
        {
            _verticalVelocity = -0.5f;
        }
    }

    private void ApplyMovement()
    {
        _moveDirection = new Vector3(_moveInput.x, _moveDirection.y, _moveInput.y);
        _characterController.Move(_moveDirection * Time.deltaTime * (_isRunning ? runSpeed : walkSpeed));
    }
    
}
