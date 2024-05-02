using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public static int onSelectMatHash = Shader.PropertyToID("_OutLineOn");
    protected SpriteRenderer spriteRenderer;
    protected Material material;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }


    public virtual void EnterCursor()
    {
        material.SetInt(onSelectMatHash, 1);
        Debug.Log(1);
    }
    public virtual void StayCursor()
    {

    }
    public virtual void ExitCursor()
    {
        material.SetInt(onSelectMatHash, 0);
        Debug.Log(0);
    }
    public virtual void MouseDown(Vector2 mousePos)
    {
        Debug.Log(transform.gameObject.name);
    }
    public virtual void MouseUp(Vector2 mousePos)
    {

    }

}
