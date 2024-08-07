using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SimpleUI : MonoBehaviour, IMovableUI
{
    [SerializeField] private Vector2 _hidePos;
    [SerializeField] private Vector2 _showPos;
    [SerializeField] private AnimationCurve _showCurve;
    [SerializeField] private AnimationCurve _hideCurve;
    [SerializeField] private float _moveTime;

    protected RectTransform _rectTransform;
    private Sequence _moveSeq;
    private Action _action;

    protected virtual void Awake()
    {
        _rectTransform = transform as RectTransform;
    }

    public virtual Action Move(bool value)
    {
        Vector2 startPos = value ? _hidePos : _showPos;
        Vector2 endPos = value ? _showPos : _hidePos;
        if (_rectTransform.anchoredPosition == endPos) return null;

        AnimationCurve curve = value ? _showCurve : _hideCurve;

        _rectTransform.anchoredPosition = startPos;
        _rectTransform.DOAnchorPos(endPos, _moveTime).SetEase(curve)
            .OnComplete(() => _action?.Invoke());

        return _action;
    }
}
