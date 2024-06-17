using Crogen.PowerfulInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoSingleton<InputManager>
{
    public InputReader inputReader;
    public Vector2 mouseWorldPos;
    public Vector2 mouseScenePos;

    private void Update()
    {
        mouseScenePos = Input.mousePosition;
        mouseWorldPos = UnityEngine.Camera.main.ScreenToWorldPoint(mouseScenePos);
    }
}
