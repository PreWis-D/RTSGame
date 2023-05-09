using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;

    [Header("Horizontal motion")]
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _accelerationInput = 2f;
    [SerializeField] private float _damping = 15f;
    private float _speed;

    [Header("Vertical motion - zooming")]
    [SerializeField] private float _stepSize = 2f;
    [SerializeField] private float _zoomDampening = 7.5f;
    [SerializeField] private float _minHeight = 5f;
    [SerializeField] private float _maxHeight = 50f;
    [SerializeField] private float _zoomSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float _maxRotationSpeed = 1f;
    [SerializeField] private float _minIncline = 340;
    [SerializeField] private float _maxIncline = 20;

    [Header("Screen edge motion")]
    [SerializeField][Range(0f, 0.1f)] private float _edgeTolerance = 0.05f;
    [SerializeField] private bool _isUseScreenEdge = true;

    private Transform _cameraTransform;

    private Vector3 _targetPosition;
    private float _zoomHeight;
    private Vector3 _horizontalVelocity;
    private Vector3 _lastPosition;

    private void Awake()
    {
        _cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        _zoomHeight = _cameraTransform.localPosition.y;
        _cameraTransform.LookAt(this.transform);

        _lastPosition = this.transform.position;
        _inputHandler.ZoomCamera += OnZoomCamera;
    }

    private void OnDisable()
    {
        _inputHandler.ZoomCamera -= OnZoomCamera;
    }

    private void Update()
    {
        GetKeyboardMovement();
        if (_isUseScreenEdge)
            CheckMouseAtScreenEdge();

        UpdateVelocity();
        UpdateRotateCamera();
        UpdateCameraPosition();
        UpdateBasePosition();
    }

    private void UpdateVelocity()
    {
        _horizontalVelocity = (this.transform.position - _lastPosition) / Time.deltaTime;
        _horizontalVelocity.y = 0;
        _lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = _inputHandler.Actions.Input.MovementCamera.ReadValue<Vector2>().x
            * GetCameraRight()
            + _inputHandler.Actions.Input.MovementCamera.ReadValue<Vector2>().y
            * GetCameraForward();

        inputValue = inputValue.normalized;
        if (_inputHandler.IsAccelerateCamera)
            inputValue *= _accelerationInput;

        if (inputValue.sqrMagnitude > 0.1f)
            _targetPosition += inputValue;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = _cameraTransform.right;
        right.y = 0;
        return right;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = _cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private void UpdateBasePosition()
    {
        if (_targetPosition.sqrMagnitude > 0.1f)
        {
            _speed = Mathf.Lerp(_speed, _maxSpeed, Time.deltaTime * _acceleration);
            transform.position += _targetPosition * _speed * Time.deltaTime;
        }
        else
        {
            _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * _damping);
            transform.position += _horizontalVelocity * Time.deltaTime;
        }

        _targetPosition = Vector3.zero;
    }

    private void UpdateRotateCamera()
    {
        if (_inputHandler.IsAllowRotateCamera == false)
            return;

        float valueX = _inputHandler.Actions.Input.RotateCamera.ReadValue<Vector2>().x;
        float valueY = _inputHandler.Actions.Input.RotateCamera.ReadValue<Vector2>().y;

        transform.rotation = Quaternion.Euler(valueY * _maxRotationSpeed + transform.rotation.eulerAngles.x,
            valueX * _maxRotationSpeed + transform.rotation.eulerAngles.y,
            0f);

        if (transform.eulerAngles.x > _maxIncline && transform.eulerAngles.x < _maxIncline * 2)
            transform.rotation = Quaternion.Euler(_maxIncline, transform.eulerAngles.y, 0f);
        else if (transform.eulerAngles.x < _minIncline && transform.eulerAngles.x > _minIncline / 2)
            transform.rotation = Quaternion.Euler(_minIncline, transform.eulerAngles.y, 0f);
    }

    private void OnZoomCamera(Vector2 inputValue)
    {
        float value = -inputValue.y / 100f;

        if (Mathf.Abs(value) > 0.1f)
        {
            _zoomHeight = _cameraTransform.localPosition.y + value * _stepSize;
            if (_zoomHeight < _minHeight)
                _zoomHeight = _minHeight;
            else if (_zoomHeight > _maxHeight)
                _zoomHeight = _maxHeight;

        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(_cameraTransform.localPosition.x, _zoomHeight, _cameraTransform.localPosition.z);
        zoomTarget -= _zoomSpeed * (_zoomHeight - _cameraTransform.localPosition.y) * Vector3.forward;

        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, zoomTarget, Time.deltaTime * _zoomDampening);
        _cameraTransform.LookAt(this.transform);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < _edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - _edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        if (mousePosition.y < _edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - _edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        if (_inputHandler.IsAccelerateCamera)
            moveDirection *= _accelerationInput;

        _targetPosition += moveDirection;
    }
}
