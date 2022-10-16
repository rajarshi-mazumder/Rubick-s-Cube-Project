using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundCube : MonoBehaviour
{
    private float _rotationX;
    private float _rotationY;
    [SerializeField]
    private float _distanceFromTarget;
    [SerializeField]
    private float _mouseSensitivity = 3.0f;
    [SerializeField]
    private float _mouseScrollSensitivity = 3.0f;
    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.3f;

    public GameObject target;
    [SerializeField]
    
    private Camera MainCamera;
    void Start()
    {
        target = GameObject.Find("Target");
        MainCamera = GetComponent<Camera>();
        Drag();
    }

   
    void Update()
    {
        if (Input.GetMouseButton(1))
            Drag();
        if(Input.GetAxis("Mouse ScrollWheel")>0)
        {
            ZoomIn();
        }
        if(Input.GetAxis("Mouse ScrollWheel")<0)
        {
            ZoomOut();
        }
    }

    void Drag()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationY += mouseX;
        _rotationX -= mouseY;

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = target.transform.position - transform.forward * _distanceFromTarget;
 
    }
    void ZoomIn()
    {
        var FOV = MainCamera.fieldOfView;
        if(FOV>30)
        {
            FOV -= _mouseScrollSensitivity ;
            MainCamera.fieldOfView = FOV;
        }
    }
    void ZoomOut()
    {
        var FOV = MainCamera.fieldOfView;
        if (FOV < 80)
        {
            FOV += _mouseScrollSensitivity ;
            MainCamera.fieldOfView = FOV;
        }
    }
}
