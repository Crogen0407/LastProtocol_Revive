using System;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.UI.GridLayoutGroup;
using System.Security.Cryptography;

[Serializable]
public enum ResourceType
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
    CopperCable,
    GoldCable,
    GlassCable,
    SolarPanel,
    Battery,
    Plastic,
    Engine,


    Count
}