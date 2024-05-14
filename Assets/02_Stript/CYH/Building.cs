using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ResourceAndCount
{
    public Resource resource;
    public int count;
}

public class Building : MonoBehaviour
{
    public List<ResourceAndCount> productResource;
    public float delayTime = 1f;
    public int MaxLevel = 100;
    public int CurrentLevel=1;
    
    //Event
    public UnityEvent OnEnableEvent;
    public UnityEvent OnDisableEvent;

    public void DestroyBuilding()
    {
        OnDisableEvent?.Invoke();
        Destroy(gameObject);
        
    }
}
