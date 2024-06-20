using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMode
{
    Default,
    Setting
}

public class GameManager : MonoSingleton<GameManager>
{
    public PlayerMode PlayerMode = PlayerMode.Default;
}
