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
    private TargetIcon _targetIcon;
    private float _currentMakeTime = 0f;

    private Dictionary<Resource, int> _currentMakeResourceRecipeDictionary = null;
    private Coroutine _moveCoroutine;

    public override void OnMouseClick()
    {
        base.OnMouseClick();

        UIManager.Instance.OpenSatelliteData(this);
    }

    public void Move(Vector2 pos)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
            _targetIcon.Init(transform, pos);
        };
        _moveCoroutine = StartCoroutine(MoveCoroutine(pos));
    }

    private IEnumerator MoveCoroutine(Vector2 targetPos)
    {
        _targetIcon.SetActive(true);
        _targetIcon.Init(transform, targetPos);
        float targetDis = (targetPos - (Vector2)transform.position).magnitude;
        float time = targetDis / _speed;
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
        _moveCoroutine = null;
        _targetIcon.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();

        _targetIcon = transform.Find("TargetIcon").GetComponent<TargetIcon>();
    }

    //테스트
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
            Debug.Log($"{resource}는 만들 수 없는 자원입니다");
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

        //자원이 있는지 검사
        bool flag = false;
        foreach (Resource keyResource in resources)
        {
            if (GetResource(keyResource) < _currentMakeResourceRecipeDictionary[keyResource])
            {
                Debug.Log($"{keyResource}이 부족\n" +
                    $"요구개수: {_currentMakeResourceRecipeDictionary[keyResource]}");
                flag = true;
            }
        }
        if (flag) return;

        //자원을 빼고 생산품 추가
        foreach (Resource keyResource in resources)
        {
            SubtractResource(keyResource, _currentMakeResourceRecipeDictionary[keyResource]);
        }
        AddResource(_makeResource, 1);
        _currentMakeTime = _makeTime;
    }
}
