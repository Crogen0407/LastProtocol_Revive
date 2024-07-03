using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : ResourceStorage
{
    [SerializeField]
    private PlanetSO planetSO;

    protected override void Awake()
    {
        base.Awake();

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            if (resourceType == ResourceType.None || resourceType == ResourceType.Count) continue;
            SetResourceAmount(resourceType, 10000);
        }
    }
}
