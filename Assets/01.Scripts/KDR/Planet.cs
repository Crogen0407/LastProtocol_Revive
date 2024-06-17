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
    }
}
