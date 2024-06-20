using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayMode
{
    Default = 1,
    Setting = 2,
    PosMove = 4
}

public class GameManager : MonoSingleton<GameManager>
{
    public PlayMode PlayMode = PlayMode.Default;
}
