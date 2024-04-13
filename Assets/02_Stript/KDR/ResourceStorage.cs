using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public int[] resourceAmountArr;

    private SpriteRenderer spriteRenderer;
    private Material material;
    private int onSelectMatHash;

    public virtual void Awake()
    {
        resourceAmountArr = new int[(int)Resource.Count];

        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        onSelectMatHash = Shader.PropertyToID("_OutLineOn");
    }

    public int GetResource(Resource resource)
    {
        return resourceAmountArr[(int)resource];
    }
    public void SetResource(Resource resource, int count)
    {
        resourceAmountArr[(int)resource] = count;
    }
    public void AddResource(Resource resource, int count)
    {
        resourceAmountArr[(int)resource] += count;
    }
    public bool SubtractResource(Resource resource, int count)
    {
        if (resourceAmountArr[(int)resource] < count) return false;

        resourceAmountArr[(int)resource] -= count;
        return true;
    }


    private void OnMouseEnter()
    {
        material.SetInt(onSelectMatHash, 1);
    }
    private void OnMouseExit()
    {
        material.SetInt(onSelectMatHash, 0);
    }
    private void OnMouseDown()
    {
        Debug.Log("마우스 클릭!");
    }
}
