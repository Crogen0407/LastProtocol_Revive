using System;
using UnityEngine;

public class MouseManager : MonoSingleton<MouseManager>
{
    [SerializeField] private LayerMask whatIsSelectable;
    [SerializeField] private float clickTime = 0;
    [SerializeField] private float clickStayTime = 0.2f;
    [SerializeField] private Vector2 clickPosition = Vector2.zero;
    [SerializeField] private float clickMoveRadius = 10;

    private Selectable currentSelectedTarget;
    private Vector2 prevMousePos;

    public bool isSelectable;
    public bool isDrag = false;
    public bool isClicked = false;

    public Action<Vector2> OnDragEvent;

    private void Awake()
    {
        InputManager.Instance.inputReader.MouseDownEvent += HandleMouseDownEvent;
        InputManager.Instance.inputReader.MouseUpEvent += HandleMouseUpEvent;
    }

    private void HandleMouseDownEvent()
    {
        isDrag = false;
        isClicked = true;
        clickTime = Time.time;
        clickPosition = InputManager.Instance.mouseScenePos;
    }

    private void HandleMouseUpEvent()
    {
        isClicked = false;


        if (isDrag == false)
        {
            if (GameManager.Instance.PlayMode == PlayMode.PosMove)
            {
                Satellite satellite = currentSelectedTarget as Satellite;
                satellite.Move(InputManager.Instance.mouseWorldPos);
                UIManager.Instance.OpenSatelliteData(satellite);
            }

            if (GameManager.Instance.PlayMode == PlayMode.Default)
            {
                currentSelectedTarget?.OnMouseClick();
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.PlayMode == PlayMode.Setting) return;

        CheckSelect();
        CheckDrag();
        CheckWheel();
    }

    private void CheckWheel()
    {
        float wheelValue = Input.GetAxisRaw("Mouse ScrollWheel");

        CameraManager.Instance.Zoom(-wheelValue);
    }

    private void CheckDrag()
    {
        bool isTimeOut = clickTime + clickStayTime < Time.time;
        bool isPosOut
            = (clickPosition - InputManager.Instance.mouseScenePos).magnitude > clickMoveRadius;

        if ((isTimeOut || isPosOut) && isClicked)
        {
            if (isDrag == false)
            {
                prevMousePos = InputManager.Instance.mouseScenePos;
                isDrag = true;
            }
            OnDragEvent?.Invoke(prevMousePos - InputManager.Instance.mouseScenePos);
            prevMousePos = InputManager.Instance.mouseScenePos;
        }
    }

    private void CheckSelect()
    {
        if (GameManager.Instance.PlayMode == PlayMode.PosMove) return;

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
            UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector3.forward, UnityEngine.Camera.main.farClipPlane, whatIsSelectable))
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
