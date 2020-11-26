using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class UIPopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;

    private void Start()
    {
        CloseCanvas();
    }

    public void OpenCanvas(string _title, string _content)
    {
        titleText.text = _title;
        contentText.text = _content;
        SetCanvasGroup(true);
    }

    public void CloseCanvas()
    {
        SetCanvasGroup(false);
    }

    private void SetCanvasGroup(bool _OnOff)
    {
        canvasGroup.alpha = _OnOff ? 1 : 0;
        canvasGroup.blocksRaycasts = _OnOff;
        canvasGroup.interactable = _OnOff;
    }
}
