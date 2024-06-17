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
    public float makingTime = -1f;

    private Dictionary<Resource, int> recipeResourceCountDictionary;
    public Dictionary<Resource, int> RecipeResourceCountDictionary
    {
        get
        {
            if (makingTime == -1) return null;
            if (recipeResourceCountDictionary == null)
            {
                recipeResourceCountDictionary = new Dictionary<Resource, int>();
                foreach (Resource resource in recipe)
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
