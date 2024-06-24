using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIcon : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private readonly int RotationHash = Shader.PropertyToID("_Rotate");
    private Transform _satelliteTrm;
    private Vector3 _position;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        
    }

    public void Init(Transform onwer, Vector3 targetPos)
    {
        _satelliteTrm = onwer;
        _position = targetPos;

        _lineRenderer.SetPosition(1, targetPos);
        Vector2 moveDir = (targetPos - onwer.position).normalized;

        float rotation = Mathf.Atan2(moveDir.y, moveDir.x);
        _lineRenderer.material.SetFloat(RotationHash, rotation);
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, _satelliteTrm.position);
        transform.position = _position;
    }

    public void SetActive(bool value)
        =>gameObject.SetActive(value);
}
