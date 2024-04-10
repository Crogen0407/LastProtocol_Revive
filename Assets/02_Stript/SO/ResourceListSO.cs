using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceList", menuName = "SO/ResourceList")]
public class ResourceListSO : ScriptableObject
{
    public ResourceSO[] resourceList;

    [ContextMenu("SettingResources")]
    public void SettingResources()
    {
        resourceList = new ResourceSO[(int)Resource.Count];
        Resource[] resourceSOList = Enum.GetValues(typeof(Resource)) as Resource[];
        for(int i = 0; i < resourceList.Length; i++)
        {
            resourceList[i].resource = resourceSOList[i];
            //string path;

            //AssetDatabase.CreateAsset()
        }
    }
}
