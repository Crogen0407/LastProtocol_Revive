using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class ResourceListUI : SimpleUI
{
    [SerializeField] private ResourceCountUI _resourceCountPrefab;
    private Dictionary<ResourceType, ResourceCountUI> resourceCountUIDictionary 
        = new Dictionary<ResourceType, ResourceCountUI>();
    private ScrollRect _scrollRect;
    private RectTransform _listTrm;
    private ResourceStorage _currentResourceStorage;

    protected override void Awake()
    {
        base.Awake();
        _scrollRect = GetComponent<ScrollRect>();
        _listTrm = _scrollRect.content;
        _listTrm.sizeDelta = new Vector2(500, ((int)ResourceType.Count - 1) * 70 - 20);
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            if (resourceType == ResourceType.None || resourceType == ResourceType.Count) continue;

            ResourceCountUI resourceCountUI = Instantiate(_resourceCountPrefab, _listTrm);
            resourceCountUI.Init(resourceType);
            resourceCountUIDictionary.Add(resourceType, resourceCountUI);
        }
    }

    public void SetResourcePanel(ResourceStorage resourceStorage)
    {
        if (_currentResourceStorage != null)
            _currentResourceStorage.resourceChangedEvent -= HandleResourceChangedEvent;

        _currentResourceStorage = resourceStorage;
        _currentResourceStorage.resourceChangedEvent += HandleResourceChangedEvent;

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            if (resourceType == ResourceType.None || resourceType == ResourceType.Count) continue;
            resourceCountUIDictionary[resourceType]
                .SetCount(_currentResourceStorage.GetResourceAmount(resourceType));
        }
    }

    private void HandleResourceChangedEvent(ResourceType resource, int count)
    {
        resourceCountUIDictionary[resource].SetCount(count);
    }
}
