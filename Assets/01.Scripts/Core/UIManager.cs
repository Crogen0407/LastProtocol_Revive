using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private DataUI _dataUI;

    public void OpenSatelliteData(Satellite satellite)
    {
        _dataUI.ShowSatellitePanel(satellite);
        CameraManager.Instance.SetForcus(satellite.transform);
    }
    public void CloseSatelliteData()
    {
        _dataUI.HideSatellitePanel();
    }
}
