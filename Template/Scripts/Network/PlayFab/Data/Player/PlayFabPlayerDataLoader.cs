using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabPlayerDataLoader : PlayFabDataLoader<PlayFabPlayerDataLoader>
{
    [SerializeField] private PlayFabPlayerDataNetwork playFabPlayerDataNetwork;

    public PlayFabPlayerData data { get; set; } = new PlayFabPlayerData();

    public void SaveUserCustomize(Action _routineSuccess)
    {
        playFabPlayerDataNetwork.SetUserCustomizeData
        (
            data.UserCustomize,
            (_) =>
            {
                Debug.Log("UserCustomize Data Saved !");
                _routineSuccess();
            }
        );
    }

    public void SaveUserInformation(Action _routineSuccess)
    {
        playFabPlayerDataNetwork.SetUserInformationData
        (
            data.UserInformation,
            (_) =>
            {
                Debug.Log("Userinformation Data Saved !");
                _routineSuccess();
            }
        );

    }

    
    public void SaveCoperationInformation(Action _routuneSuccess)
    {
        //PlayFabAddressableNetwork.
    }
}
