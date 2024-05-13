using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using DialogSystem;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance = null;

    public Dialog dialog = null;
    [SerializeField] private List<DialogSO> dialogSO = new List<DialogSO>();

    public bool isReadingDialog { get; private set; }


    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        //디버그용도
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("밍");
            StartDialog(dialogSO[0]);
        }
    }

    public void StartDialog(DialogSO dialogSO)
    {
        if (dialogSO != null)
        {
            isReadingDialog = true;
            dialog.gameObject.SetActive(true);
            dialog.Init(dialogSO);
            dialog.StartDialog();
        }
    }

    public void StartDialog(int idx)
    {
        if (dialogSO[idx] != null)
        {
            isReadingDialog = true;
            dialog.gameObject.SetActive(true);
            dialog.Init(dialogSO[idx]);
            dialog.StartDialog();
        }
    }

    public void EndDialog()
    {
        dialog.StopCoroutine("WaitNextScript");
        dialog.StopCoroutine("ReadingLine");
        isReadingDialog = false;
    }
}
