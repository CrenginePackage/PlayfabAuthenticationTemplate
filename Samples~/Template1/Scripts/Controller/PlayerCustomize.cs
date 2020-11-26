using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomize : MonoBehaviour
{
    public AvatarPreset maleAvatarPreset;
    public AvatarPreset femaleAvatarPreset;
    public AvatarPreset specialAvatarPreset;

    //public GameObject[] avatarObjects;
    //[SerializeField] private PlayerAnimation playerAnimation;
    //[SerializeField] private PlayerMove playerMove;
    //[SerializeField] private PlayerTeleport playerTeleport;

    [HideInInspector] public int userGender;
    private GameObject currentAvatarObject;
    

    // Temporary
    public void ApplyCustomize(int _userGender)
    {
        //ApplyGender(_userGender);
    }

    public void ApplyCustomize(UserCustomize _userCustomize, SpecialAvatarNumber _specialAvatarNumber)
    {
        // SetAvatar((int)userCustomize.HairColor);
        // playerAnimation.SetAnimator(avatarObjects[(int)userCustomize.HairColor].GetComponent<Animator>());

        //ApplyGender(userCustomize.userGender);
        currentAvatarObject = SetAvatar(_userCustomize, _specialAvatarNumber);
        //playerAnimation.SetAnimator(currentAvatarObject.GetComponent<Animator>());
        //playerTeleport.SetDissolveEffect(currentAvatarObject.GetComponent<DissolveEffect>());

        //if (playerMove != null)
        //    playerMove.animator = currentAvatarObject.GetComponent<Animator>();
    }

    //private void ApplyGender(int _userGender, )
    //{
    //    currentAvatarObject = SetAvatar(_userGender);
    //    playerAnimation.SetAnimator(avatarObjects[(int)_userGender].GetComponent<Animator>());
    //    userGender = _userGender;
    //}

    //private void ApplyAvatarPreset(int _user)
    //{


    //}

    

    private GameObject SetAvatar(UserCustomize _userCustomize, SpecialAvatarNumber _specialAvatarNumber)
    {
        if (_specialAvatarNumber == SpecialAvatarNumber.General)
        {
            CloseGenderAvatarPreview(specialAvatarPreset);

            if (_userCustomize.userGender == 0)
            {
                CloseGenderAvatarPreview(femaleAvatarPreset);
                return SetAvatarPreview(maleAvatarPreset, _userCustomize.avatarPreset, _userCustomize.upperColor);
            }
            else
            {
                CloseGenderAvatarPreview(maleAvatarPreset);
                return SetAvatarPreview(femaleAvatarPreset, _userCustomize.avatarPreset, _userCustomize.upperColor);
            }
        }
        else
        {
            CloseGenderAvatarPreview(femaleAvatarPreset);
            CloseGenderAvatarPreview(maleAvatarPreset);
            return SetAvatarPreview(specialAvatarPreset, ((int)_specialAvatarNumber - 1), CustomizeColor.White);
        }
        

    }

    private GameObject SetAvatarPreview(AvatarPreset _genderAvatarPreset, int _presetNumber, CustomizeColor _upperColor)
    {
        for (int i = 0; i < _genderAvatarPreset.avatarPresets.Length; i++)
        {
            if (_presetNumber == i)
            {
                _genderAvatarPreset.avatarPresets[i].SetActive(true);
                _genderAvatarPreset.avatarPresets[i].GetComponent<AvatarColorRenderer>().ChangeUpperColor(_upperColor);
                continue;
            }
            _genderAvatarPreset.avatarPresets[i].SetActive(false);
        }

        return _genderAvatarPreset.avatarPresets[_presetNumber];
    }

    private void CloseGenderAvatarPreview(AvatarPreset _genderAvatarPreset)
    {
        for (int i = 0; i < _genderAvatarPreset.avatarPresets.Length; i++)
        {
            _genderAvatarPreset.avatarPresets[i].SetActive(false);
        }
    }


}
