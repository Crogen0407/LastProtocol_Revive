using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    private float _scenePixelToWorldPos;
    private void Start()
    {
        MouseManager.Instance.OnDragEvent += HandleOnDragEvent;
        float screenHight = Screen.height;
        float cameraHight = CameraManager.Instance.CurrentVCamera.m_Lens.OrthographicSize * 2;
        _scenePixelToWorldPos = cameraHight / screenHight;
    }

    private void HandleOnDragEvent(Vector2 movement)
    {
        transform.position += (Vector3)movement * _scenePixelToWorldPos;
    }
}
