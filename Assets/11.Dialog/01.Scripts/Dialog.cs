using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using DialogSystem;

public class Dialog : MonoBehaviour
{
    private Transform canvas;
    private Image background;
    private RectTransform dialogParent;
    private RectTransform bottomBar;
    private TextMeshProUGUI nameTxt;
    private TextMeshProUGUI detailsTxt;
    private GameObject characterImg;

    private DialogSO curDialog;
    private ScriptSO curScript;
    private string curTxt;
    private bool isReadingTxt = false;

    private bool isOptionSelected;
    [SerializeField] private float bottomBarMoveTime = 0.5f;

    private Coroutine readlineCoroutine;
    private Coroutine readtextCoroutine;
    private Coroutine waitNextScriptCorotine;

    private bool isBottomBarReveal = false;


    private void Update()
    {
        if (isReadingTxt == true && GetInput())
            StartCoroutine("SkipLine");
    }


    public void StartDialog()
    {
        curScript = curDialog.scriptList[0];

        if (readlineCoroutine != null)
            StopCoroutine(readlineCoroutine);

        readlineCoroutine = StartCoroutine("ReadSingleLine");
    }

    IEnumerator ReadSingleLine()
    {
        if (waitNextScriptCorotine != null)
            StopCoroutine(waitNextScriptCorotine);

        //cameraSettings = curScript.cameraSettings;

        NoScriptSO n = curScript as NoScriptSO;
        OptionSO o = curScript as OptionSO;
        NormalScriptSO normal = curScript as NormalScriptSO;

        if (n || o)
        {
            if (isBottomBarReveal == true)
            {
                HideBottomBar();
                yield return new WaitForSeconds(bottomBarMoveTime);
            }

            if (n && n.character.imgPrefab != null)
            {
                characterImg = Instantiate(n.character.imgPrefab, dialogParent);
                characterImg.transform.SetSiblingIndex(0);                              //캐릭터는 대화창 뒤에 있어야 도미

                if (n.character.characterAnimation == null)
                {
                    n.character.characterAnimation.Init(characterImg.GetComponent<RectTransform>());
                    n.character.characterAnimation.Animation();
                }
            }
        }
        else if (normal)
        {
            if (isBottomBarReveal == false)
            {
                RevealBottomBar();
                yield return new WaitForSeconds(bottomBarMoveTime);
            }
            SetImage();
            nameTxt.SetText(normal.character.name);

            if (characterImg != null && normal.character.characterAnimation != null)
            {
                normal.character.characterAnimation.Init(characterImg.GetComponent<RectTransform>());
                normal.character.characterAnimation.Animation();
            }

            if (readtextCoroutine != null)
                StopCoroutine(readtextCoroutine);

            readtextCoroutine = StartCoroutine("ReadTexts");
            yield return readtextCoroutine;
        }

        if (waitNextScriptCorotine != null)
            StopCoroutine(waitNextScriptCorotine);

        waitNextScriptCorotine = StartCoroutine("WaitNextScript");
    }

    IEnumerator ReadTexts()
    {
        yield return null;
        isReadingTxt = true;

        NormalScriptSO normal = curScript as NormalScriptSO;

        for (int i = 0; i < normal.character.talkDetails.Length; i++)
        {
            curTxt += normal.character.talkDetails[i];
            detailsTxt.SetText(curTxt);

            if (normal.character.talkDetails[i] != ' ')
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator WaitNextScript()
    {
        if (readlineCoroutine != null)
        {
            StopCoroutine(readlineCoroutine);
            readlineCoroutine = null;
        }

        curTxt = "";
        isReadingTxt = false;

        NoScriptSO n = curScript as NoScriptSO;
        OptionSO o = curScript as OptionSO;
        NormalScriptSO normal = curScript as NormalScriptSO;

        if (n)
        {
            yield return new WaitUntil(() => GetInput());

            curScript = n.nextScript;
            if(characterImg != null)
            {
                Destroy(characterImg);
            }
        }
        else if (normal)
        {
            yield return new WaitUntil(() => GetInput());

            curScript = normal.nextScript;
            if (characterImg != null)
            {
                Destroy(characterImg);
            }
        }
        else if (o)
        {
            SetOption();

            yield return new WaitUntil(() => isOptionSelected == true); //|| isOptionSelectTimeEnd == true);
            isOptionSelected = false;
        }
        yield return null;

        if (readlineCoroutine != null)
            StopCoroutine(readlineCoroutine);
        if (readtextCoroutine != null)
            StopCoroutine(readtextCoroutine);

        if (curScript != null)
        {
            if (readlineCoroutine != null)
                StopCoroutine(readlineCoroutine);

            readlineCoroutine = StartCoroutine("ReadSingleLine");
        }
        else
            OnEndDialog();
    }

    IEnumerator SkipLine()
    {
        StopCoroutine("ReadSingleLine");
        StopCoroutine("ReadTexts");

        NormalScriptSO normal = curScript as NormalScriptSO;
        curTxt = normal.character.talkDetails;
        detailsTxt.SetText(curTxt);

        yield return new WaitForSeconds(0.1f);
        StartCoroutine("WaitNextScript");
    }

    private void HideBottomBar()
    {
        bottomBar.DOAnchorPosX(-1920f, bottomBarMoveTime)
        .OnComplete(() =>
         {
             curTxt = "";
             nameTxt.SetText("");
             detailsTxt.SetText("");
             isBottomBarReveal = false;
         });
    }

    private void RevealBottomBar()
    {
        curTxt = "";
        nameTxt.SetText("");
        detailsTxt.SetText("");

        bottomBar.DOAnchorPosX(0f, bottomBarMoveTime)
            .OnComplete(() => isBottomBarReveal = true);
    }

    private void SetImage()
    {
        NormalScriptSO normal = curScript as NormalScriptSO;

        if (normal && normal.character.imgPrefab != null)
        {
            characterImg = Instantiate(normal.character.imgPrefab, dialogParent);
            characterImg.transform.SetSiblingIndex(0);                              //캐릭터는 대화창 뒤에 있어야 도미
        }
    }

    private void SetOption()
    {
        OptionSO option = curScript as OptionSO;

        for (int i = option.options.Count - 1; i >= 0; i--)
        {
            Button optionBtn = Instantiate(option.optionPf, dialogParent.Find("Options")).GetComponent<Button>();
            optionBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(option.options[i].detail);

            int index = i;

            optionBtn.onClick.AddListener(() =>
            {
                for (int i = 0; i < dialogParent.Find("Options").childCount; i++)
                {
                    Destroy(dialogParent.Find("Options").GetChild(i).gameObject);
                }
                curScript = option.options[index].nextScript;
                isOptionSelected = true;
            });
        }
    }

    private bool GetInput()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
    }

    private void OnEndDialog()
    {
        Destroy(dialogParent.gameObject);
        DialogManager.instance.EndDialog();
    }

    public void Init(DialogSO dialog)
    {
        canvas = GameObject.Find("Canvas").transform;   //캔버스 이름 바꾸면 얘도 바꿔줘
        curDialog = dialog;

        dialogParent = Instantiate(dialog.dialogBackground, canvas).GetComponent<RectTransform>();
        bottomBar = dialogParent.transform.Find("BottomBar").GetComponent<RectTransform>();
        nameTxt = bottomBar.Find("NameTxt").GetComponent<TextMeshProUGUI>();
        detailsTxt = bottomBar.Find("Details").GetComponent<TextMeshProUGUI>();
    }
}
