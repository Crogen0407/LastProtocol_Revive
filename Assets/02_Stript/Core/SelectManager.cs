using Crogen.PowerfulInput;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoSingleton<SelectManager>
{
    [SerializeField] private LayerMask whatIsSelectable;

    private Selectable currentSelectedTarget;

    public bool isSelectable;

    private void Awake()
    {
        InputManager.Instance.inputReader.MouseDownEvent += HandleMouseDownEvent;
        InputManager.Instance.inputReader.MouseUpEvent += HandleMouseUpEvent;
    }

    private void HandleMouseDownEvent()
    {
        currentSelectedTarget?.MouseDown(InputManager.Instance.mouseWorldPos);
    }

    private void HandleMouseUpEvent()
    {
        currentSelectedTarget?.MouseUp(InputManager.Instance.mouseWorldPos);
    }

    private void Update()
    {
        if (isSelectable == false && IsSelectable(out Selectable selectable))
        {
            if (currentSelectedTarget != selectable)
            {
                currentSelectedTarget?.ExitCursor();
                currentSelectedTarget = selectable;
                currentSelectedTarget.EnterCursor();
            }
        }
        else
        {
            if (currentSelectedTarget != null)
            {
                currentSelectedTarget.ExitCursor();
                currentSelectedTarget = null;
            }
        }
        currentSelectedTarget?.StayCursor();
    }

    public bool IsSelectable(out Selectable selectable)
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector3.forward, Camera.main.farClipPlane, whatIsSelectable))
        {
            selectable = hit.transform.GetComponent<Selectable>();
            return true;
        }
        else
        {
            selectable = null;
            return false;
        }
    }

}
