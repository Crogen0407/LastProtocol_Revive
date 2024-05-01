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

    private Dictionary<Resource, int> currentMakeResourceRecipeDictionary = null;



    [SerializeField]
    private bool isMakeable = true;

    protected override void Awake()
    {
        base.Awake();
    }



    //테스트
    private void Start()
    {
        Initialize();
    }

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
        ChangeMakeResource(Resource.Semiconductor);
    }

    public void ChangeMakeResource(Resource resource)
    {
        if (ResourceManager.Instance.GetResourceSO(makeResource).makingTime == -1)
        {
            Debug.Log($"{resource}는 만들 수 없는 자원입니다");
            return;
        } 

        makeResource = resource;
        makeTime = ResourceManager.Instance.GetResourceSO(makeResource).makingTime;
        currentMakeTime = makeTime;
        currentMakeResourceRecipeDictionary = 
            ResourceManager.Instance.GetResourceSO(makeResource).RecipeResourceCountDictionary;
    }

    private void Make()
    {
        if (currentMakeResourceRecipeDictionary == null) return;
        var resources = currentMakeResourceRecipeDictionary.Keys;

        //자원이 있는지 검사
        bool flag = false;
        foreach (Resource keyResource in resources)
        {
            if (GetResource(keyResource) < currentMakeResourceRecipeDictionary[keyResource])
            {
                Debug.Log($"{keyResource}이 부족\n" +
                    $"요구개수: {currentMakeResourceRecipeDictionary[keyResource]}");
                flag = true;
            }
        }
        if (flag) return;

        //자원을 빼고 생산품 추가
        foreach (Resource keyResource in resources)
        {
            SubtractResource(keyResource, currentMakeResourceRecipeDictionary[keyResource]);
        }
        AddResource(makeResource, 1);
        currentMakeTime = makeTime;
    }
}
