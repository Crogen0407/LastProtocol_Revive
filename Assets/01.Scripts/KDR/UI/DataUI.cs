using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataUI : MonoBehaviour
{
    [SerializeField] private SimpleUI simpleUI;

    public void OpenResource(ResourceStorage resourceStorage)
    {
        (simpleUI as IMovableUI).Move(true);
    }
}
