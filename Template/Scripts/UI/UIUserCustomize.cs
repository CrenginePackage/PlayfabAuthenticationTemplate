 using UnityEngine;
using System.Collections.Generic;

public class UIUserCustomize : MonoBehaviour
{
    //[SerializeField] private GameObject[] preivewObjects;
    [Header("Default Avatars")]
    [SerializeField] private AvatarPreset maleAvatarPreset;
    [SerializeField] private AvatarPreset femaleAvatarPreset;
    [SerializeField] private GameObject maleAvatarButtons;
    [SerializeField] private GameObject femaleAvatarButtons;

    [Header("Special Avatars")]
    [SerializeField] private AvatarPreset specialAvatarPreset;
    [SerializeField] private Sprite[] specialAvatarImages;

    [Header("UI Component")]
    [SerializeField] private UISpecialAvatarButton specialAvatarButtonPrefab;
    [SerializeField] private Transform specialAvatarButtonTransform;
    [SerializeField] private GameObject specialAvatarComponents;

    private void Start()
    {
        GetUserCustomize();
        //SetSpecialAvatarButton(PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarsAllowed);
    }

    public void GetUserCustomize()
    {
        //Debug.Log(PlayFabPlayerDataLoader.Instance.data.UserCustomize.userGender);
        //UpdateCustomize(PlayFabPlayerDataLoader.Instance.data.UserCustomize);
        SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize, PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected);
    }

    private void SetSpecialAvatarButton(List<int> _specialAvatarAllowed)
    {
        if (_specialAvatarAllowed.Count <= 0)
        {
            specialAvatarComponents.SetActive(false);
            return;
        }

        for (int i = 0; i < _specialAvatarAllowed.Count; i++)
        {
            GameObject button = Instantiate(specialAvatarButtonPrefab.gameObject, specialAvatarButtonTransform);
            button.name = "SpecialAvatar_" + _specialAvatarAllowed[i];
            button.GetComponent<UISpecialAvatarButton>().Init(this, (SpecialAvatarNumber)_specialAvatarAllowed[i], specialAvatarImages[_specialAvatarAllowed[i]]);
        }
    }

    //private void UpdateCustomize(UserCustomize userCustomize)
    //{
    //    SetPreview(userCustomize.userGender, userCustomize.avatarPreset, userCustomize.upperColor, );
    //}

    private void SetSpecialAvatar(SpecialAvatarNumber specialAvatarNumber)
    {
        CloseAvatarPreview(femaleAvatarPreset);
        CloseAvatarPreview(maleAvatarPreset);

        SetAvatarPreview(specialAvatarPreset, (int)specialAvatarNumber, CustomizeColor.White);
    }

    private void SetPreview(UserCustomize userCustomize, SpecialAvatarNumber _userSpecialAvatarSelected)
    {
        if (_userSpecialAvatarSelected == SpecialAvatarNumber.General)
        {
            if (userCustomize.userGender == 0)
            {
                CloseAvatarPreview(femaleAvatarPreset);
                SetAvatarPreview(maleAvatarPreset, userCustomize.avatarPreset, userCustomize.upperColor);
            }
            else
            {
                CloseAvatarPreview(maleAvatarPreset);
                SetAvatarPreview(femaleAvatarPreset, userCustomize.avatarPreset, userCustomize.upperColor);
            }

            CloseAvatarPreview(specialAvatarPreset);
            ChangeAvatarPresetButtons(userCustomize.userGender);

            return;
        }
        else
        {
            CloseAvatarPreview(femaleAvatarPreset);
            CloseAvatarPreview(maleAvatarPreset);

            SetAvatarPreview(specialAvatarPreset, ((int)_userSpecialAvatarSelected) - 1, CustomizeColor.White);
        }
    }


    //private void SetPreview(int _genderNumber, int _avatarPreset, CustomizeColor _upperColor, SpecialAvatarNumber specialAvatarNumber)
    //{
    //    if (_genderNumber == 0)
    //    {
    //        CloseGenderAvatarPreview(femaleAvatarPreset);
    //        SetAvatarPreview(maleAvatarPreset, _avatarPreset, _upperColor);
    //    }
    //    else
    //    {
    //        CloseGenderAvatarPreview(maleAvatarPreset);
    //        SetAvatarPreview(femaleAvatarPreset, _avatarPreset, _upperColor);
    //    }

    //    ChangeAvatarPresetButtons(_genderNumber);    
    //    return;
    //}   


    private void ChangeAvatarPresetButtons(int _userGender)
    {
        if (_userGender == 0)
        {
            maleAvatarButtons.SetActive(true);
            femaleAvatarButtons.SetActive(false);
            return;
        }
        maleAvatarButtons.SetActive(false);
        femaleAvatarButtons.SetActive(true);
    }

    private void SetAvatarPreview(AvatarPreset _genderAvatarPreset, int _avatarPreset, CustomizeColor _upperColor)
    {
        for (int i = 0; i < _genderAvatarPreset.avatarPresets.Length; i++)
        {
            if (_avatarPreset == i)
            {
                _genderAvatarPreset.avatarPresets[i].SetActive(true);
                _genderAvatarPreset.avatarPresets[i].GetComponent<AvatarColorRenderer>().ChangeUpperColor(_upperColor);
                continue;
            }
            _genderAvatarPreset.avatarPresets[i].SetActive(false);
        }
    }

    private void CloseAvatarPreview(AvatarPreset _genderAvatarPreset)
    {
        for (int i = 0; i < _genderAvatarPreset.avatarPresets.Length; i++)
        {
            _genderAvatarPreset.avatarPresets[i].SetActive(false);
        }
    }

    public void ChangeUserGender(int _userGender)
    {
        PlayFabPlayerDataLoader.Instance.data.UserCustomize.userGender = _userGender;
        PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected = 0;
        SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize, PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected);
        //SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize.userGender, PlayFabPlayerDataLoader.Instance.data.UserCustomize.avatarPreset, PlayFabPlayerDataLoader.Instance.data.UserCustomize.upperColor);
    }


    public void ChangeAvatarPreset(int _avatarPreset)
    {
        PlayFabPlayerDataLoader.Instance.data.UserCustomize.avatarPreset = _avatarPreset;
        PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected = 0;
        SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize, PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected);
        //SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize.userGender, PlayFabPlayerDataLoader.Instance.data.UserCustomize.avatarPreset, PlayFabPlayerDataLoader.Instance.data.UserCustomize.upperColor);
    }

    public void ChangeUpperColor(int _customizeColor)
    {
        PlayFabPlayerDataLoader.Instance.data.UserCustomize.upperColor = (CustomizeColor) _customizeColor;
        PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected = 0;
        SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize, PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected);
        //SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize.userGender, PlayFabPlayerDataLoader.Instance.data.UserCustomize.avatarPreset, PlayFabPlayerDataLoader.Instance.data.UserCustomize.upperColor);
    }

    public void ChangeSpecialAvatar(SpecialAvatarNumber _specialAvatarNumber)
    {
        PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected = _specialAvatarNumber;
        SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize, PlayFabPlayerDataLoader.Instance.data.UserSpecialAvatar.SpecialAvatarSelected);
        //SetPreview(PlayFabPlayerDataLoader.Instance.data.UserCustomize.userGender, PlayFabPlayerDataLoader.Instance.data.UserCustomize.avatarPreset, PlayFabPlayerDataLoader.Instance.data.UserCustomize.upperColor);
    }
}
