using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UICanvasItem
{
    public string canvasName;
    [Range(0,1)]
    public float alphaValue = 0;
    public CanvasGroup canvasGroup;
    public UnityEvent OnCanvasOpen;

    public void OpenCanvas()
    {
        SetCanvasGroup(true);
        OnCanvasOpen.Invoke();
    }

    public void CloseCanvas()
    {
        SetCanvasGroup(false);
    }


    private void SetCanvasGroup(bool OnOff)
    {
        canvasGroup.alpha = OnOff ? 1 : alphaValue;
        canvasGroup.blocksRaycasts = OnOff;
        canvasGroup.interactable = OnOff;
    }

}