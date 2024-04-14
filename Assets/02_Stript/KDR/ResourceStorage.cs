using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public struct ResourceAndCount
{
    [HideInInspector]
    public string name;
    public int count;
}

public class ResourceStorage : MonoBehaviour
{
    public ResourceAndCount[] resourceAmountArr;

    private SpriteRenderer spriteRenderer;
    private Material material;
    private int onSelectMatHash;

    public virtual void Awake()
    {
        resourceAmountArr = new ResourceAndCount[(int)Resource.Count];

        string[] names = Enum.GetNames(typeof(Resource));
        for (int i = 0; i < resourceAmountArr.Length; i++)
        {
            resourceAmountArr[i].name = names[i];
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        onSelectMatHash = Shader.PropertyToID("_OutLineOn");
    }

    public int GetResource(Resource resource)
    {
        return resourceAmountArr[(int)resource].count;
    }
    public void SetResource(Resource resource, int count)
    {
        resourceAmountArr[(int)resource].count = count;
    }
    public void AddResource(Resource resource, int count)
    {
        resourceAmountArr[(int)resource].count += count;
    }
    public bool SubtractResource(Resource resource, int count)
    {
        if (resourceAmountArr[(int)resource].count < count) return false;

        resourceAmountArr[(int)resource].count -= count;
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
