using Crogen.PowerfulInput;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoSingleton<SelectManager>
{
    [SerializeField] private LayerMask whatIsSelectable;

    private Selectable currentSelectTarget;

    private void Awake()
    {
        InputManager.Instance.inputReader.MouseDownEvent += HandleMouseDownEvent;
        InputManager.Instance.inputReader.MouseUpEvent += HandleMouseUpEvent;
    }

    private void HandleMouseDownEvent()
    {
        currentSelectTarget?.MouseDown(InputManager.Instance.mouseWorldPos);
    }

    private void HandleMouseUpEvent()
    {
        currentSelectTarget?.MouseUp(InputManager.Instance.mouseWorldPos);
    }

    private void Update()
    {
        if (IsSelectable(out Selectable selectable))
        {
            if (currentSelectTarget != selectable)
            {
                currentSelectTarget?.ExitCursor();
                currentSelectTarget = selectable;
                currentSelectTarget.EnterCursor();
            }
        }
        else
        {
            if (currentSelectTarget != null)
            {
                currentSelectTarget.ExitCursor();
                currentSelectTarget = null;
            }
        }
        currentSelectTarget?.StayCursor();
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
