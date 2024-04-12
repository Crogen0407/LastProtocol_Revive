using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Spaceship : MonoBehaviour
{
    [Header("Spaceship Setting")]
    [SerializeField]
    private float defaultSpeed = 10;
    [SerializeField]
    private float loadingSpeed = 1f;
    [SerializeField]
    private float speedChangeSpeed = 7f;
    private float cueentSpeed;

    [Space(10)]
    [SerializeField]
    private Resource currentLoadResource;
    public Resource loadResource;
    [SerializeField]
    private int maxResourceLoadAmount = 10;
    [SerializeField]
    private int currentResourceLoadAmount = 0;
    [SerializeField]
    private float loadingTime = 0.25f;
    [SerializeField]
    private float loadingTimeDeviation = 0.05f;

    [Space(10)]
    [SerializeField]
    private float defaultRotationSpeed = 5f;
    [SerializeField]
    private float loadingRotationSpeed = 10f;
    private Quaternion targetRotation;


    [Header("Link Setting")]
    [SerializeField]
    private ResourceStorage startResourceStorage;
    [SerializeField]
    private ResourceStorage endResourceStorage;
    private ResourceStorage targetResourceStorage;
    private float linkDistance 
        => (startResourceStorage.transform.position - endResourceStorage.transform.position).magnitude;
    [SerializeField]
    private float collisionRadius;
    [SerializeField]
    private LayerMask whatIsStorageObject;

    private Coroutine resourceLoadReadyCoroutine = null;


    [SerializeField]
    private bool isGoing = false;
    [SerializeField]
    private bool isLoading = false;

    private bool canMove = true;

    private void FixedUpdate()
    {
        if (IsLinkFinish())
        {
            if (canMove)
            {
                if (isLoading == false)
                    CollisionCast();
                Move();
            }
            Rotation();

            collisionRadius = Mathf.Max(0.3f, cueentSpeed / 17);
        }
    }

    private void Update()
    {
        if (loadResource != currentLoadResource && isGoing == false)
            currentLoadResource = loadResource;
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        isLoading = false;
        isGoing = false;
        canMove = true;
        //vv나중에 지울거vv
        if (IsLinkFinish())
            targetResourceStorage = isGoing ? endResourceStorage : startResourceStorage;
        resourceLoadReadyCoroutine = null;
        currentResourceLoadAmount = 0;
    }

    #region 충돌

    private Collider2D[] collisions;
    private void CollisionCast()
    {
        collisions = Physics2D.OverlapCircleAll(transform.position, collisionRadius, whatIsStorageObject);
        foreach (Collider2D coll in collisions)
        {
            if (coll.transform == targetResourceStorage.transform)
            {
                SpaceshipCollision(coll.transform.GetComponent<ResourceStorage>());
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
    public void SpaceshipCollision(ResourceStorage resourceStorage)
    {
        if (isGoing)
        {
            resourceStorage.AddResource(currentLoadResource, currentResourceLoadAmount);
            currentResourceLoadAmount = 0;
        }
        else
        {
            if (resourceStorage.SubtractResource(currentLoadResource, maxResourceLoadAmount))
                currentResourceLoadAmount = maxResourceLoadAmount;
            else
            {
                currentResourceLoadAmount = resourceStorage.GetResource(currentLoadResource);
                resourceStorage.SetResource(currentLoadResource, 0);
            }
        }
        isGoing = !isGoing;
        targetResourceStorage = isGoing ? endResourceStorage : startResourceStorage;
        if (resourceLoadReadyCoroutine != null) StopCoroutine(resourceLoadReadyCoroutine);
        resourceLoadReadyCoroutine = StartCoroutine(ResourceLoadReadyCoroutine());
    }
    private IEnumerator ResourceLoadReadyCoroutine()
    {
        isLoading = true;
        yield return new WaitForSeconds
            (Random.Range(loadingTime - loadingTimeDeviation, loadingTime + loadingTimeDeviation));
        isLoading = false;
    }
    #endregion

    private void Move()
    {
        cueentSpeed = Mathf.Lerp(cueentSpeed, (isLoading ? loadingSpeed : defaultSpeed), 
            Time.fixedDeltaTime * speedChangeSpeed);
        transform.position += transform.up * cueentSpeed * Time.fixedDeltaTime;
    }

    private void Rotation()
    {
        targetRotation = Quaternion.LookRotation(
                -Vector3.forward,
                targetResourceStorage.transform.position - transform.position);
        transform.rotation = 
            Quaternion.Lerp(transform.rotation, targetRotation,
            (isLoading ? loadingRotationSpeed : defaultRotationSpeed) * Time.fixedDeltaTime);
    }

    private bool IsLinkFinish()
    {
        return endResourceStorage != null && startResourceStorage != null;
    }


    public void SetStartTrm(ResourceStorage storage)
    {
        startResourceStorage = storage;
    }
    public void SetEndtTrm(ResourceStorage storage)
    {
        endResourceStorage = storage;
    }
}
