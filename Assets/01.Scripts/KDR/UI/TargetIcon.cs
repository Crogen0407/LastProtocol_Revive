using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIcon : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private readonly int RotationHash = Shader.PropertyToID("_Rotate");
    private Transform _stalliteTrm;

    public void Init(Transform stallite, Vector3 targetPos)
    {
        _stalliteTrm = stallite;
        transform.position = targetPos;

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(1, transform.position);
        Vector2 moveDir = (transform.position - _stalliteTrm.position).normalized;
        float rotation = -Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        _lineRenderer.material.SetFloat(RotationHash, rotation);
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, _stalliteTrm.position);
    }
}
