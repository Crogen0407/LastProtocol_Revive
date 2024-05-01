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
        Debug.Log(material);
    }


    public virtual void EnterCursor()
    {
        material.SetInteger(onSelectMatHash, 1);
    }
    public virtual void StayCursor()
    {

    }
    public virtual void ExitCursor()
    {
        material.SetInteger(onSelectMatHash, 0);
    }
    public virtual void MouseDown()
    {
        Debug.Log(transform.gameObject.name);
    }
    public virtual void MouseUp()
    {

    }

}
