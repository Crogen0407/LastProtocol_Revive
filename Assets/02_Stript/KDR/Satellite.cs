using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : ResourceStorage
{
    [SerializeField]
    private Resource makeResource;
    [SerializeField]
    private float makeTime = 1f;
    private float currentMakeTime = 0f;

    private bool isMakeable;

    private void Update()
    {
        if (isMakeable)
        {
            if (currentMakeTime > 0)
            {
                currentMakeTime -= Time.deltaTime;
            }
            else
            {
                Make();
            }
        }
    }
    public void Initialize()
    {
        makeResource = Resource.None;
    }

    public void ChangeMakeResource(Resource resource)
    {
        makeResource = resource;
        //makeTime = resource.makingTime;
        currentMakeTime = 0;
    }

    private void Make()
    {
        /*if (GetResource())
        {
            AddResource(makeResource, 1);
            currentMakeTime = makeTime;
        }*/
    }
}
