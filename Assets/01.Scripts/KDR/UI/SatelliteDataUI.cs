using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteDataUI : SimpleUI
{
    [SerializeField] private Button _moveBtn, _moveExitBtn;
    [SerializeField] private SimpleUI _moveModeText;
    private Satellite _satellite;

    protected override void Awake()
    {
        base.Awake();
        _moveBtn.onClick.AddListener(ShowMoveMode);
        _moveExitBtn.onClick.AddListener(HideMoveMode);
    }

    public void Init(Satellite satellite)
    {
        _satellite = satellite;
        _moveModeText.Move(false);
        _moveExitBtn.GetComponent<IMovableUI>().Move(false);
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
        UIManager.Instance.OpenSatelliteData(_satellite);
        _moveModeText.Move(false);
        _moveExitBtn.GetComponent<IMovableUI>().Move(false);
    }
}
