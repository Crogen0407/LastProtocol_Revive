using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceAmountUI : MonoBehaviour
{
    private string _resourceName;
    [SerializeField] private TextMeshProUGUI _resourceCountText;
    [SerializeField] private Image _resourceIcon;

    private int _targetAmount;
    private int _currentAmount = 0;

    public void Init(ResourceType resource)
    {
        ResourceSO resourceSO = ResourceManager.Instance.GetResourceSO(resource);
        _resourceName = resourceSO.name;
        _resourceIcon.sprite = resourceSO.sprite;
        SetCount(0);
    }

    private void Update()
    {
        if (_currentAmount != _targetAmount)
        {
            _currentAmount++;
            _resourceCountText.SetText($"{_resourceName} : {_currentAmount}");
        }
    }

    public void SetCount(int amount)
    {
        _targetAmount = amount;
        gameObject.SetActive(amount != 0);
    }
} 
