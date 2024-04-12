using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Resource
{
    //고체
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

    //액체
    Water,
    Oil,
    LiquidNitrogen,

    //기체
    Hydrogen,
    Oxygen,
    MethaneGas,

    //물체
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
