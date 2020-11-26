using System.Collections;
using System.Collections.Generic;
//using Crengine.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerCustomize playerCustomize;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Text displayNameText;
    

    public void Init(PlayFabPlayerData playerData, bool _disableMove)
    {
        ApplyCustomize(playerData.UserCustomize, playerData.UserSpecialAvatar.SpecialAvatarSelected);
        playerMove.disableMove = _disableMove;
        displayNameText.text = playerData.UserInformation.UserName;
        //SetLocation(_location);
    }

    private void ApplyCustomize(UserCustomize _userCustomize, SpecialAvatarNumber _userSpecialAvatar)
    {
        playerCustomize.ApplyCustomize(_userCustomize, _userSpecialAvatar);
    }


    //private void SetLocation(Transform _location)
    //{
    //    playerMove.transform.position = _location.position;
    //    playerMove.transform.rotation = _location.rotation;
    //}



}
