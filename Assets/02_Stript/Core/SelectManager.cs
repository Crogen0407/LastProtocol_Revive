using Crogen.PowerfulInput;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoSingleton<SelectManager>
{
    [SerializeField] private LayerMask whatIsSelectable;
    [SerializeField] private InputReader inputReader;

    private Selectable currentSelectTarget;

    private void Awake()
    {
        inputReader.MouseDownEvent += HandleMouseDownEvent;
        inputReader.MouseUpEvent += HandleMouseUpEvent;
    }

    private void HandleMouseDownEvent()
    {
        currentSelectTarget.MouseDown();
    }

    private void HandleMouseUpEvent()
    {
        currentSelectTarget.MouseUp();
    }

    private void Update()
    {
        if (IsSelectable(out Selectable selectable))
        {
            if (currentSelectTarget != selectable)
            {
                if (currentSelectTarget != null) currentSelectTarget.ExitCursor();
                currentSelectTarget = selectable;
                currentSelectTarget.EnterCursor();
            }
        }
        if (currentSelectTarget != null) currentSelectTarget.StayCursor();
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
