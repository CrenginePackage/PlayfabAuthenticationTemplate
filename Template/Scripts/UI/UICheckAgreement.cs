using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICheckAgreement : MonoBehaviour
{
    [SerializeField] private Toggle[] toggleButtons;
    [SerializeField] private Button nextButton;
    [SerializeField] private Image nextButtonImage;
    [SerializeField] private Color disableColor;

    public void ResetToggles()
    {
        SetToggleButtons(false);
        SetNextButton(false);
    }

    public void CheckAgreement()
    {
        if (CheckToggleButtons())
        {
            SetNextButton(true);
            return;
        }

        SetNextButton(false);
    }

    private bool CheckToggleButtons()
    {
        bool checkValue = true;

        for (int i = 0; i < toggleButtons.Length; i++)
        {
            checkValue = checkValue & toggleButtons[i].isOn;
        }

        return checkValue;
    }

    private void SetToggleButtons(bool OnOff)
    {
        for (int i = 0; i < toggleButtons.Length; i++)
        {
            toggleButtons[i].isOn = OnOff;
        }
    }

    private void SetNextButton(bool OnOff)
    {
        nextButton.interactable = OnOff;
        nextButtonImage.color = OnOff ? Color.white : disableColor;
    }

}
