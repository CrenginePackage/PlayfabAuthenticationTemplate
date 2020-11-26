using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialAvatarButton : MonoBehaviour
{
    public SpecialAvatarNumber specialAvatarNumber { get; set; }
    public UIUserCustomize uiUserCustomize { get; set; }

    [Header("UI Component")]
    [SerializeField] private Image specialAvatarImage;

    public void Init(UIUserCustomize _uiUserCustomize, SpecialAvatarNumber _specialAvatarNumber, Sprite _specialAvatarImage)
    {
        this.uiUserCustomize = _uiUserCustomize;
        this.specialAvatarNumber = _specialAvatarNumber;
        specialAvatarImage.sprite = _specialAvatarImage;
    }

    public void OnClick()
    {
        uiUserCustomize.ChangeSpecialAvatar(specialAvatarNumber);
    }
}
