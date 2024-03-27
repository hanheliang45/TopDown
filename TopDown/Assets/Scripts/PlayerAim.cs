using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAim : MonoBehaviour
{
    private PlayerController _controller;
    private Vector2 _aimInput;
    private RaycastHit _lastMouseHit;
    private RaycastHit _currentMouseHit;
    
    [Header("Aim")] 
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private Transform cameraObject;
    [SerializeField] private Transform aimObject;
    [SerializeField] private float minCameraDistance = 1;
    [SerializeField] private float maxCameraDistance = 3;
    [Range(3f, 8f)]
    [SerializeField] private float aimSensitivity = 3;
    [SerializeField] private bool isPreciseAim;

    [Header("Laser")]
    [SerializeField] private LineRenderer laser;
    
    void Start()
    {
        _controller = GetComponent<PlayerCore>().PC;
        
        _controller.Character.Aim.performed += context => _aimInput = context.ReadValue<Vector2>();
        _controller.Character.Aim.canceled += context => _aimInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyCamera();
        ApplyAim();
        ApplyLaser();
    }

    private void ApplyLaser()
    {
        Transform gunPoint = PlayerCore.Instance.GetWeaponController().GetGunPoint();
        laser.SetPosition(0, gunPoint.position);

        float gunRange = 6f;
        Vector3 direction = aimObject.position - gunPoint.position;
        laser.SetPosition(1, gunPoint.position + direction.normalized * gunRange);
    }

    private void ApplyAim()
    {
        RaycastHit hit = _currentMouseHit;
        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
        {
            Renderer targetRenderer = target.GetComponent<Renderer>();
            aimObject.position = targetRenderer == null ? hit.transform.position
                    : targetRenderer.bounds.center;
        }
        else
        {
            Vector3 mouseHitPoint = hit.point;

            aimObject.position = isPreciseAim
                ? mouseHitPoint
                : new Vector3(mouseHitPoint.x, this.transform.position.y + 1.0f, mouseHitPoint.z);
        }
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(_aimInput);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
        {
            _lastMouseHit = hitInfo;
            _currentMouseHit = hitInfo;
            return _currentMouseHit;
        }

        return _lastMouseHit;
    }


    private Vector3 DesiredCameraPosition()
    {
        Vector3 mousePosition = _currentMouseHit.point;

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float aimDistance = Mathf.Clamp(Vector3.Distance(mousePosition, transform.position),
            minCameraDistance, maxCameraDistance);
        Vector3 aimPosition = transform.position + aimDirection * aimDistance;

        return new Vector3(aimPosition.x, this.transform.position.y + 1.0f, aimPosition.z);
    }

    private void ApplyCamera()
    {
        cameraObject.position = Vector3.Lerp(cameraObject.position, DesiredCameraPosition(),
            aimSensitivity * Time.deltaTime);
    }
}
