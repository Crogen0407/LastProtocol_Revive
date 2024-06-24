using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataUI : MonoBehaviour
{
    [SerializeField] private ResourceListUI _resourceListUI;
    [SerializeField] private SatelliteDataUI _satelliteDataUI;
    [SerializeField] private Button _backBtn;
    private SimpleUI _backSimpleUI;

    private void Awake()
    {
        _backSimpleUI = _backBtn.GetComponent<SimpleUI>();
        _backBtn.onClick.AddListener(UIManager.Instance.CloseSatelliteData);
    }

    public void ShowSatellitePanel(Satellite satellite)
    {
        GameManager.Instance.PlayMode = PlayMode.Setting;
        _resourceListUI.SetResourcePanel(satellite);
        _resourceListUI.Move(true);
        _satelliteDataUI.Move(true);
        _backSimpleUI.Move(true);
        _satelliteDataUI.HideMoveMode();
        CameraManager.Instance.SetForcus(satellite.transform);
    }
    public void HideSatellitePanel()
    {
        GameManager.Instance.PlayMode = PlayMode.Default;
        _satelliteDataUI.Move(false);
        _resourceListUI.Move(false);
        _backSimpleUI.Move(false);
        CameraManager.Instance.SetForcus();
    }
}
