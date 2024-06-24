using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResourceAndCount
{
    [HideInInspector]
    public string name;
    public int count;
}

public class ResourceStorage : Selectable
{
    private Dictionary<ResourceType, int> resourceAmountDictionary
        = new Dictionary<ResourceType, int>();
    public Action<ResourceType, int> resourceChangedEvent;

    protected override void Awake()
    {
        base.Awake();

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            resourceAmountDictionary.Add(resourceType, 0);
        }
    }

    public int GetResourceAmount(ResourceType resource)
    {
        return resourceAmountDictionary[resource];
    }
    public void SetResourceAmount(ResourceType resource, int count)
    {
        resourceAmountDictionary[resource] = count;
        resourceChangedEvent?.Invoke(resource, count);
    }
    public void AddResourceAmount(ResourceType resource, int count)
    {
        resourceAmountDictionary[resource] += count;
        resourceChangedEvent?.Invoke(resource, resourceAmountDictionary[resource]);
    }
    public bool SubtractResourceAmount(ResourceType resource, int count)
    {
        if (resourceAmountDictionary[resource] < count) return false;
            
        resourceAmountDictionary[resource] -= count;
        resourceChangedEvent?.Invoke(resource, resourceAmountDictionary[resource]);
        return true;
    }
}
