using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSO", menuName = "SO/Resource")]
public class ResourceSO : ScriptableObject
{
    public Resource resource;
    public Sprite sprite;
    public Resource[] recipe;
}
