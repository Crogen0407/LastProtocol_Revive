using System.Collections;
using System.Collections.Generic;
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

    private void SetResourcePanel(ResourceAndCount[] resourceAndCount)
    {
        //리소스 정렬
    }

    public void ShowSatellitePanel(Satellite satellite)
    {
        SetResourcePanel(satellite.resourceAmountArr);
        _resourceListUI.Move(true);
        _satelliteDataUI.Move(true);
        _backSimpleUI.Move(true);
    }
    public void HideSatellitePanel()
    {
        _satelliteDataUI.Move(false);
        _resourceListUI.Move(false);
        _backSimpleUI.Move(false);
    }
}
