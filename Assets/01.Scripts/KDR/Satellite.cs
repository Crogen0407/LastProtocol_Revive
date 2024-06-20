using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : ResourceStorage
{
    [Header("ResourceMakeSetting")]
    [SerializeField] private bool _isMakeable = true;

    [SerializeField] private Resource _makeResource;
    [SerializeField] private float _makeTime = 1f;
    [SerializeField] private float _speed = 5f;
    private float _currentMakeTime = 0f;

    private Vector2 _targetPos;


    private Dictionary<Resource, int> _currentMakeResourceRecipeDictionary = null;


    public override void OnMouseClick()
    {
        base.OnMouseClick();

        UIManager.Instance.OpenSatelliteData(this);
    }

    private IEnumerator MoveCoroutine()
    {
        float time = _targetPos.magnitude / _speed;
        float percent = 0;
        float current = 0;

        Vector2 startPos = transform.position;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector2.Lerp(startPos, _targetPos, percent);
            yield return null;
        }
    }

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
        if (_isMakeable)
        {
            if (_currentMakeTime > 0)
            {
                _currentMakeTime -= Time.deltaTime;
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
        if (ResourceManager.Instance.GetResourceSO(_makeResource).makingTime == -1)
        {
            Debug.Log($"{resource}�� ���� �� ���� �ڿ��Դϴ�");
            return;
        } 

        _makeResource = resource;
        _makeTime = ResourceManager.Instance.GetResourceSO(_makeResource).makingTime;
        _currentMakeTime = _makeTime;
        _currentMakeResourceRecipeDictionary = 
            ResourceManager.Instance.GetResourceSO(_makeResource).RecipeResourceCountDictionary;
    }

    private void Make()
    {
        if (_currentMakeResourceRecipeDictionary == null) return;
        var resources = _currentMakeResourceRecipeDictionary.Keys;

        //�ڿ��� �ִ��� �˻�
        bool flag = false;
        foreach (Resource keyResource in resources)
        {
            if (GetResource(keyResource) < _currentMakeResourceRecipeDictionary[keyResource])
            {
                Debug.Log($"{keyResource}�� ����\n" +
                    $"�䱸����: {_currentMakeResourceRecipeDictionary[keyResource]}");
                flag = true;
            }
        }
        if (flag) return;

        //�ڿ��� ���� ����ǰ �߰�
        foreach (Resource keyResource in resources)
        {
            SubtractResource(keyResource, _currentMakeResourceRecipeDictionary[keyResource]);
        }
        AddResource(_makeResource, 1);
        _currentMakeTime = _makeTime;
    }
}
