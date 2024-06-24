using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteDataUI : SimpleUI
{
    [SerializeField] private Button _moveBtn, _moveExitBtn;
    [SerializeField] private SimpleUI _moveModeText;
    private DataUI _dataUI;

    protected override void Awake()
    {
        base.Awake();
        _moveBtn.onClick.AddListener(ShowMoveMode);
        _moveExitBtn.onClick.AddListener(HideMoveMode);
    }

    public void Init(DataUI dataUI)
    {
        _dataUI = dataUI;
    }

    private void ShowMoveMode()
    {
        UIManager.Instance.CloseSatelliteData();
        GameManager.Instance.PlayMode = PlayMode.PosMove;
        _moveModeText.Move(true);
        _moveExitBtn.GetComponent<IMovableUI>().Move(true);
    }
    public void HideMoveMode()
    {
        _moveModeText.Move(false);
        _moveExitBtn.GetComponent<IMovableUI>().Move(false);
    }
}
