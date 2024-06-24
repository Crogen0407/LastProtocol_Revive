using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCountUI : MonoBehaviour
{
    private string _resourceName;
    [SerializeField] private TextMeshProUGUI _resourceCountText;
    [SerializeField] private Image _resourceIcon;


    public void Init(ResourceType resource)
    {
        ResourceSO resourceSO = ResourceManager.Instance.GetResourceSO(resource);
        _resourceName = resourceSO.name;
        _resourceIcon.sprite = resourceSO.sprite;
        SetCount(0);
    }

    public void SetCount(int count)
    {
        if (count == 0)
            gameObject.SetActive(false);
        else
        {
            _resourceCountText.SetText($"{_resourceName} : {count}");
            gameObject.SetActive(true);
        }
    }
} 
