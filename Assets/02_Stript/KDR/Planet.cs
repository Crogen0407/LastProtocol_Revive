using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private PlanetSO planetSO;

    private Dictionary<Resource, int> resouceAmountDictionery = new Dictionary<Resource, int>();


    public void AddReSource(Resource resourcen, int amount)
    {
        resouceAmountDictionery[resourcen] += amount;
    }
    public void SetReSource(Resource resourcen, int amount)
    {
        resouceAmountDictionery[resourcen] = amount;
    }
    public void SubtractReSource(Resource resourcen, int amount)
    {
        resouceAmountDictionery[resourcen] -= amount;
    }

    public int GetReSourceAmount(Resource resourcen)
    {
        return resouceAmountDictionery[resourcen];
    }
}
