using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spaceship : Selectable
{
    [Header("Spaceship Setting")]
    [SerializeField] private float _defaultSpeed = 10;
    [SerializeField] private float _loadingSpeed = 1f;
    [SerializeField] private float _speedChangeSpeed = 7f;
    private float _cueentSpeed;

    [Space(10)]
    [SerializeField] private ResourceType _currentLoadResource;
    public ResourceType _loadResource;
    [SerializeField] int _maxResourceLoadAmount = 10;
    [SerializeField] private int _currentResourceLoadAmount = 0;
    [SerializeField] private float _loadingTime = 0.25f;
    [SerializeField] private float _loadingTimeDeviation = 0.05f;

    [Space(10)]
    [SerializeField] private float _defaultRotationSpeed = 5f;
    [SerializeField] private float _loadingRotationSpeed = 10f;
    private Quaternion _targetRotation;


    [Header("Link Setting")]
    [SerializeField] private ResourceStorage _startResourceStorage;
    [SerializeField] private ResourceStorage _endResourceStorage;
    private ResourceStorage _targetResourceStorage;
    private float _linkDistance
        => (_startResourceStorage.transform.position - _endResourceStorage.transform.position).magnitude;
    private float _collisionRadius;
    [SerializeField] private LayerMask _whatIsStorageObject;

    private Coroutine _resourceLoadReadyCoroutine = null;


    [SerializeField] private bool _isGoing = false;
    [SerializeField] private bool _isLoading = false;

    private bool _canMove = true;

    private void FixedUpdate()
    {
        if (IsLinkFinish())
        {
            if (_canMove)
            {
                if (_isLoading == false)
                    CollisionCast();
                Move();
            }
            Rotation();

            _collisionRadius = Mathf.Max(0.3f, _cueentSpeed / 17);
        }
    }

    private void Update()
    {
        if (_loadResource != _currentLoadResource && _isGoing == false)
            _currentLoadResource = _loadResource;
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _isLoading = false;
        _isGoing = false;
        _canMove = true;
        //vv나중에 지울거vv
        if (IsLinkFinish())
            _targetResourceStorage = _isGoing ? _endResourceStorage : _startResourceStorage;
        _resourceLoadReadyCoroutine = null;
        _currentResourceLoadAmount = 0;
    }

    #region 충돌

    private Collider2D[] collisions;
    private void CollisionCast()
    {
        collisions = Physics2D.OverlapCircleAll(transform.position, _collisionRadius, _whatIsStorageObject);
        foreach (Collider2D coll in collisions)
        {
            if (coll.transform == _targetResourceStorage.transform)
            {
                SpaceshipCollision(coll.transform.GetComponent<ResourceStorage>());
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _collisionRadius);
    }
    public void SpaceshipCollision(ResourceStorage resourceStorage)
    {
        if (_currentLoadResource != ResourceType.None && _currentLoadResource != ResourceType.Count)
        {
            if (_isGoing)
            {
                resourceStorage.AddResourceAmount(_currentLoadResource, _currentResourceLoadAmount);
                _currentResourceLoadAmount = 0;
            }
            else
            {
                if (resourceStorage.SubtractResourceAmount(_currentLoadResource, _maxResourceLoadAmount))
                    _currentResourceLoadAmount = _maxResourceLoadAmount;
                else
                {
                    _currentResourceLoadAmount = resourceStorage.GetResourceAmount(_currentLoadResource);
                    resourceStorage.SetResourceAmount(_currentLoadResource, 0);
                }
            }
        }
        _isGoing = !_isGoing;
        _targetResourceStorage = _isGoing ? _endResourceStorage : _startResourceStorage;
        if (_resourceLoadReadyCoroutine != null) StopCoroutine(_resourceLoadReadyCoroutine);
        _resourceLoadReadyCoroutine = StartCoroutine(ResourceLoadReadyCoroutine());
    }
    private IEnumerator ResourceLoadReadyCoroutine()
    {
        _isLoading = true;
        yield return new WaitForSeconds
            (Random.Range(_loadingTime - _loadingTimeDeviation, _loadingTime + _loadingTimeDeviation));
        _isLoading = false;
    }
    #endregion

    private void Move()
    {
        _cueentSpeed = Mathf.Lerp(_cueentSpeed, (_isLoading ? _loadingSpeed : _defaultSpeed),
            Time.fixedDeltaTime * _speedChangeSpeed);
        transform.position += transform.up * _cueentSpeed * Time.fixedDeltaTime;
    }

    private void Rotation()
    {
        _targetRotation = Quaternion.LookRotation(
                Vector3.back,
                _targetResourceStorage.transform.position - transform.position);
        transform.rotation =
            Quaternion.Lerp(transform.rotation, _targetRotation,
            (_isLoading ? _loadingRotationSpeed : _defaultRotationSpeed) * Time.fixedDeltaTime);
    }

    private bool IsLinkFinish()
    {
        return _endResourceStorage != null && _startResourceStorage != null;
    }


    public void SetStartTrm(ResourceStorage storage)
    {
        _startResourceStorage = storage;
    }
    public void SetEndtTrm(ResourceStorage storage)
    {
        _endResourceStorage = storage;
    }
}
