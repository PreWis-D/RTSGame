using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public ActionScheme Actions { get; private set; }
    public bool IsAllowRotateCamera { get; private set; }
    public bool IsAccelerateCamera { get; private set; }

    public event Action<Vector2> ZoomCamera;
    public event Action LeftMouseButtonClecked;

    private void Awake()
    {
        Actions = new ActionScheme();
        Actions.Input.AllowRotateCamera.started += OnAllowRotateCameraClick;
        Actions.Input.AllowRotateCamera.canceled += OnAllowRotateCameraClick;
        Actions.Input.ZoomCamera.performed += OnZoomCamera;
        Actions.Input.AccelerateCamera.started += OnAccelerateCameraClick;
        Actions.Input.AccelerateCamera.canceled += OnAccelerateCameraClick;
        Actions.Input.LeftButtonMouse.started += OnLeftButtonMouseClicked;
    }

    private void OnEnable()
    {
        Actions.Input.Enable();
    }

    private void OnDisable()
    {
        Actions.Input.AllowRotateCamera.started -= OnAllowRotateCameraClick;
        Actions.Input.AllowRotateCamera.canceled -= OnAllowRotateCameraClick;
        Actions.Input.ZoomCamera.performed -= OnZoomCamera;
        Actions.Input.AccelerateCamera.started -= OnAccelerateCameraClick;
        Actions.Input.AccelerateCamera.canceled -= OnAccelerateCameraClick;
        Actions.Input.LeftButtonMouse.started -= OnLeftButtonMouseClicked;
        Actions.Disable();
    }

    private void OnAllowRotateCameraClick(InputAction.CallbackContext context)
    {
        IsAllowRotateCamera = context.ReadValueAsButton();
    }

    private void OnZoomCamera(InputAction.CallbackContext context)
    {
        ZoomCamera?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnAccelerateCameraClick(InputAction.CallbackContext context)
    {
        IsAccelerateCamera = context.ReadValueAsButton();
    }
    
    private void OnLeftButtonMouseClicked(InputAction.CallbackContext context)
    {
        LeftMouseButtonClecked?.Invoke();
    }
}
