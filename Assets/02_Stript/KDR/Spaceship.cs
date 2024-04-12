using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spaceship : MonoBehaviour
{
    [Header("Spaceship Setting")]
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private Resource currentLoadResource;
    [SerializeField]
    private int maxResourceLoad = 10;
    [SerializeField]
    private int currentResourceLoad = 0;


    [Header("Link Setting")]
    [SerializeField]
    private Transform startTrm;
    [SerializeField]
    private Transform endTrm;
    private float linkDistance => (startTrm.position - endTrm.position).magnitude;

    private Coroutine resourceLoadReadyCoroutine;


    private bool isGoing = true;
    private bool canMove = true;

    private void FixedUpdate()
    {
        Move();
        Rotation();
    }

    private void Move()
    {
        if (canMove == false) return;
        transform.position = transform.forward * speed * Time.fixedDeltaTime;
    }

    private void Rotation()
    {
        if (canMove == false) return;
        transform.rotation = 
            Quaternion.LookRotation(isGoing ? endTrm.position : startTrm.position, Vector3.up);
    }

    public void SetStartTrm(Transform trm)
    {
        startTrm = trm;
    }
    public void SetEndtTrm(Transform trm)
    {
        endTrm = trm;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<ResourceStorage>(out ResourceStorage resourceStorage))
        {
            if (resourceStorage.SubtractResource(currentLoadResource, maxResourceLoad))
            {
                currentResourceLoad = maxResourceLoad;
            }
            else
            {
                currentResourceLoad = resourceStorage.GetResource(currentLoadResource);
                resourceStorage.SetResource(currentLoadResource, 0);
            }
            resourceLoadReadyCoroutine = StartCoroutine(ResourceLoadReadyCoroutine());
        }
    }

    private IEnumerator ResourceLoadReadyCoroutine()
    {
        canMove = false;

        yield return new WaitForSeconds(2f);

        isGoing = !isGoing;
        canMove = true;
    }
}
