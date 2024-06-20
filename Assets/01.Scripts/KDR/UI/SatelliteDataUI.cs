using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteDataUI : SimpleUI
{
    [SerializeField] private Button _moveBtn;
    [SerializeField] private SimpleUI _moveModeText;

    protected override void Awake()
    {
        base.Awake();
        _moveBtn.onClick.AddListener(ShowMoveMode);
    }

    private void ShowMoveMode()
    {
        UIManager.Instance.CloseSatelliteData();
        GameManager.Instance.PlayMode = PlayMode.PosMove;
        _moveModeText.Move(true);
    }
    public void HideMoveMode()
    {
        _moveModeText.Move(false);
    }
}
