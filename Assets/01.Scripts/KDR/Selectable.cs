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
    }
    public virtual void StayCursor()
    {

    }
    public virtual void ExitCursor()
    {
        material.SetInt(onSelectMatHash, 0);
    }
    public virtual void MouseClick(Vector2 mousePos)
    {

    }
}
