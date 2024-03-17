using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _controller;

    private Vector2 _moveInput;
    private Vector2 _aimInput;

    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private float _verticalVelocity;
    private Vector3 _lookingDirection;
    private Animator _animator;
    private bool _isRunning;
    
    [Header("Movement info")]
    [@SerializeField] private float walkSpeed = 5;
    [@SerializeField] private float runSpeed = 8;

    [Header("Aim")] 
    [@SerializeField] private LayerMask aimLayerMask;
    [@SerializeField] private Transform aimObject;
    
    void Awake()
    {
        _controller = new PlayerController();

        _characterController = this.GetComponent<CharacterController>();
        _animator = this.GetComponentInChildren<Animator>();
            
        AssignInput();
    }
    
    private void AssignInput()
    {
        _controller.Character.Fire.performed += context => Shoot();
        _controller.Character.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
        _controller.Character.Move.canceled += context => _moveInput = Vector2.zero;
        _controller.Character.Aim.performed += context => _aimInput = context.ReadValue<Vector2>();
        _controller.Character.Aim.canceled += context => _aimInput = Vector2.zero;
        _controller.Character.Run.performed += context => _isRunning = true;
        _controller.Character.Run.canceled += context => _isRunning = false;
    }

    private void Shoot()
    {
        _animator.SetTrigger("Fire");
    }

    void Update()
    {
        ApplyMovement();
        ApplyGravity();
        ApplyAim();

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

    private void ApplyAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(_aimInput);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            _lookingDirection = hitInfo.point - transform.position;
            _lookingDirection.y = 0f;
            _lookingDirection.Normalize();

            transform.forward = _lookingDirection;

            aimObject.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }
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

    private void OnEnable()
    {
        _controller.Enable();
    }

    private void OnDisable()
    {
        _controller.Disable();
    }
}
