using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Resource
{
    //��ü
    None,
    Iron,
    Copper,
    Gold,
    Aluminum,
    Titanium,
    Silicon,
    Carbon,
    Uranium,
    Lithium,
    Neodymium,

    //��ü
    Water,
    Oil,
    LiquidNitrogen,

    //��ü
    Hydrogen,
    Oxygen,
    MethaneGas,

    //��ü
    Glass,
    Semiconductor,
    CopperWire,
    GoldWire,
    GlassWire,
    SolarPanel,
    Battery,
    Plastic,
    Engine,


    Count
}

[CreateAssetMenu(fileName = "ResourceSO", menuName = "SO/Resource")]
public class ResourceSO : ScriptableObject
{
    public Resource resource;
    public Sprite sprite;
    public Resource[] recipe;
}
