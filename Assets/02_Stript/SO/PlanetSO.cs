using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetSO", menuName = "SO/Planet")]
public class PlanetSO : ScriptableObject
{
    public int gravityLevel = 1;
    public bool air = true;
    public float terraformingProgress;
    public float terraformingProgressMul;
    public int controlTowerLevel = 1;
    public int storyIndex;
    public int storyCheck;
    public int planetIndex;
    public BuildingInfo[] BuildingInfos;
}


[Serializable]
public struct BuildingInfo
{
    //public Tower type;
    public int level;
    public bool canUseBuilding;
    public bool resourceProductionNormalize;
}
