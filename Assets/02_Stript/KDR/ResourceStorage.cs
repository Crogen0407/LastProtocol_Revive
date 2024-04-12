using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    private int[] resourceAmountArr;

    public virtual void Awake()
    {
        resourceAmountArr = new int[(int)Resource.Count];
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
}
