using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField] private LayerMask placementLayerMask;

    public event Action OnCkicked, OnExit;

    private void OnEnable()
    {
        _inputHandler.LeftMouseButtonClecked += OnLeftMouseButtonClecked;
    }

    private void OnDisable()
    {
        _inputHandler.LeftMouseButtonClecked -= OnLeftMouseButtonClecked;
    }

    private void Update()
    {
        if (_inputHandler.IsAccelerateCamera)
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject(); 

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    private void OnLeftMouseButtonClecked()
    {
        OnCkicked?.Invoke();
    }
}
