using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSO", menuName = "SO/Resource")]
public class ResourceSO : ScriptableObject
{
    public ResourceType resource;
    public Sprite sprite;
    new public string name;
    public ResourceType[] recipe;
    public float makingTime = -1f;

    private Dictionary<ResourceType, int> recipeResourceCountDictionary;
    public Dictionary<ResourceType, int> RecipeResourceCountDictionary
    {
        get
        {
            if (makingTime == -1) return null;
            if (recipeResourceCountDictionary == null)
            {
                recipeResourceCountDictionary = new Dictionary<ResourceType, int>();
                foreach (ResourceType resource in recipe)
                {
                    if (recipeResourceCountDictionary.TryAdd(resource, 1) == false)
                    {
                        recipeResourceCountDictionary[resource]++;
                    }
                }
            }
            return recipeResourceCountDictionary;
        }
    }
}
