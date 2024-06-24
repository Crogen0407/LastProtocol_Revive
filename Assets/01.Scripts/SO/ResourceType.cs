using System;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.UI.GridLayoutGroup;
using System.Security.Cryptography;

[Serializable]
public enum ResourceType
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
    CopperCable,
    GoldCable,
    GlassCable,
    SolarPanel,
    Battery,
    Plastic,
    Engine,


    Count
}