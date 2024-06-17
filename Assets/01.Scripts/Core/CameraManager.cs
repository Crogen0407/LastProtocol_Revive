using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _playerVCam;
    [SerializeField] private Vector2 cameraZoomClamp;
    [SerializeField] private float targetCameraZoomValue;
    [SerializeField] private float zoomValueChangeSpeed;

    [HideInInspector] public CinemachineVirtualCamera CurrentVCamera;

    private void Awake()
    {
        CurrentVCamera = _playerVCam;
        targetCameraZoomValue = CurrentVCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        CurrentVCamera.m_Lens.OrthographicSize 
            = Mathf.Lerp(CurrentVCamera.m_Lens.OrthographicSize, targetCameraZoomValue, Time.deltaTime * zoomValueChangeSpeed);
    }

    public void Zoom(float value)
    {
        targetCameraZoomValue += value;
        targetCameraZoomValue
            = Mathf.Clamp(targetCameraZoomValue + value, cameraZoomClamp.x, cameraZoomClamp.y);
    }
}
