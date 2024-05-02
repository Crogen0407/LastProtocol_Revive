using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : ResourceStorage
{
    public static Satellite satellite;

    [Header("ResourceMakeSetting")]
    [SerializeField] private Resource makeResource;
    [SerializeField] private float makeTime = 1f;
    private float currentMakeTime = 0f;


    [Header("ResourceMakeSetting")]
    [SerializeField] private float speed = 5f;

    private Vector2 targetPos;


    private Dictionary<Resource, int> currentMakeResourceRecipeDictionary = null;

    Coroutine moveCoroutine;

    public override void MouseDown(Vector2 mousePos)
    {
        base.MouseDown(mousePos);

        if (satellite != null) return;

        if (satellite == this)
        {
            targetPos = mousePos;

            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCoroutine());

            return;
        }
        satellite = this;
    }

    private IEnumerator MoveCoroutine()
    {
        float time = targetPos.magnitude / speed;
        float percent = 0;
        float current = 0;

        Vector2 startPos = transform.position;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector2.Lerp(startPos, targetPos, percent);
            yield return null;
        }
    }

    [SerializeField]
    private bool isMakeable = true;

    protected override void Awake()
    {
        base.Awake();
    }



    //�׽�Ʈ
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
            Debug.Log($"{resource}�� ���� �� ���� �ڿ��Դϴ�");
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

        //�ڿ��� �ִ��� �˻�
        bool flag = false;
        foreach (Resource keyResource in resources)
        {
            if (GetResource(keyResource) < currentMakeResourceRecipeDictionary[keyResource])
            {
                Debug.Log($"{keyResource}�� ����\n" +
                    $"�䱸����: {currentMakeResourceRecipeDictionary[keyResource]}");
                flag = true;
            }
        }
        if (flag) return;

        //�ڿ��� ���� ����ǰ �߰�
        foreach (Resource keyResource in resources)
        {
            SubtractResource(keyResource, currentMakeResourceRecipeDictionary[keyResource]);
        }
        AddResource(makeResource, 1);
        currentMakeTime = makeTime;
    }
}
