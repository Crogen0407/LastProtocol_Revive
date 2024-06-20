using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraPos : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;
    private Tween tween;

    private void Start()
    {
        MouseManager.Instance.OnDragEvent += HandleOnDragEvent;
        InputManager.Instance.inputReader.MouseDownEvent += HandleMouseDownEvent;
        InputManager.Instance.inputReader.MouseUpEvent += HandleMouseUpEvent;
    }
     
    private void Update()
    {
        transform.position += _velocity;
    }

    private void HandleMouseUpEvent()
    {
        tween = DOTween.To(() => _velocity, value => _velocity = value, Vector3.zero, 0.2f).SetEase(Ease.OutSine);
    }

    private void HandleMouseDownEvent()
    {
        tween.Kill();
        _velocity = Vector3.zero;
    }

    private void HandleOnDragEvent(Vector2 movement)
    {
        float screenHight = Screen.height;
        float cameraHight = CameraManager.Instance.CurrentVCamera.m_Lens.OrthographicSize * 2;
        float _scenePixelToWorldPos = cameraHight / screenHight;
        _velocity = (Vector3)movement * _scenePixelToWorldPos;
    }

    public void SetParent(Transform trm, bool resetPos = false)
    {
        transform.SetParent(trm);
        if (resetPos) transform.localPosition = Vector3.zero;
    }
}
