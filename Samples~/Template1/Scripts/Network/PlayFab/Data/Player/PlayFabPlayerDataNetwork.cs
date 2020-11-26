using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabPlayerDataNetwork : PlayFabBaseNetwork
{
    public void GetAllUserData(string _ID, Action _routineSuccess)
    {

    }

    public void GetUserCustomizeData(string _ID, Action<UserCustomize> _routineSuccess)
    {
        GetUserCustomizeRequest(_ID, "UserCustomize",
            (_) =>
            {
                _routineSuccess((UserCustomize)_);
            }, ConvertUserCustomize,
            new List<string>() { "UserCustomize" });
    }

    public void GetUserInformationData(string _ID, Action<UserInformation> _routineSuccess)
    {
        GetUserInformationRequest(_ID,
            (_) =>
            {
                _routineSuccess(_);
            }, ConvertUserInformation,
            ReflectionUtility.GetProperties(typeof(UserInformation)));
    }

    private UserCustomize ConvertUserCustomize(string dataString)
    {
        return new UserCustomize(dataString);
    }

    private UserInformation ConvertUserInformation(string _userAddress, string _userName, int _userType)
    {
        return new UserInformation(_userAddress, _userName, _userType);
    }

    public void GetUserCustomizeRequest(string _ID, string dataKey, Action<IPlayFabData> _routineSuccess, Func<string, IPlayFabData> converter, List<string> _keys)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = _ID,
            Keys = _keys
        },
        (_) => { GetUserCustomizeSuccess(_, _ID, dataKey, _routineSuccess, converter, _keys); },
        (_) => { OnDebugPlayFabFailure(_, "GetUserData"); });
    }

    public void GetUserInformationRequest(string _ID, Action<UserInformation> _routineSuccess, Func<string, string, int, UserInformation> converter, List<string> _keys)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = _ID,
            Keys = _keys
        },
        (_) => { GetUserInformationSuccess(_, _ID, _keys, _routineSuccess, converter, _keys); },
        (_) => { OnDebugPlayFabFailure(_, "GetUserData"); });
    }

    private void GetUserCustomizeSuccess(GetUserDataResult result, string _ID, string dataKey, Action<IPlayFabData> _routineSuccess, Func<string, IPlayFabData> converter, List<string> _keys)
    {
        if (result.Data == null || !result.Data.ContainsKey(dataKey))
        {
            Debug.Log(dataKey + " not set");
            SetUserCustomizeData(
                new UserCustomize(),
                (_) => 
                {
                    SetDataSuccess(_);
                    GetUserCustomizeRequest(_ID, dataKey, _routineSuccess, converter, _keys);
                });
        }
        else
        {
            _routineSuccess(converter(result.Data[dataKey].Value));
        }
    }

    private void GetUserInformationSuccess(GetUserDataResult result, string _ID, List<string> dataKeys, Action<UserInformation> _routineSuccess, Func<string, string, int, UserInformation> converter, List<string> _keys)
    {
        UserInformation information = new UserInformation();

        if (result.Data == null)
        {
            Debug.Log("No data");
            SetUserInformationData(
                information,
                (_) =>
                {
                    SetDataSuccess(_);
                    GetUserInformationRequest(_ID, _routineSuccess, converter, _keys);
                });

        }
        else
        {
            Debug.Log("Data Exists");

            bool missing = false;    

            for (int i = 0; i < dataKeys.Count; i++)
            {
                if (result.Data.ContainsKey(dataKeys[i]))
                {
                    SetInformation(information, i, result.Data[dataKeys[i]].Value);
                }
                else
                {
                    Debug.Log("Data Missing : " + dataKeys[i]);
                    missing = true;
                }
            }

            if (missing)
            {
                SetUserInformationData(
                    information,
                    (_) =>
                    {
                        SetDataSuccess(_);
                        GetUserInformationRequest(_ID, _routineSuccess, converter, _keys);
                    });
            }
            else
            {
                _routineSuccess(information);
            }
        }
    }

    private void SetInformation(UserInformation info, int i, string dataString)
    {
        switch (i)
        {
            case 0:
                info.UserAddress = dataString;
                break;
            case 1:
                info.UserName = dataString;
                break;
            case 2:
                info.UserType = (UserType) int.Parse(dataString);
                break;
        }
    }

    public void SetUserCustomizeData(UserCustomize userCustomize, Action<UpdateUserDataResult> _routineSuccess)
    {
        SetUserDataRequest(userCustomize, _routineSuccess);
    }

    public void SetUserInformationData(UserInformation userInformation, Action<UpdateUserDataResult> _routineSuccess)
    {
        SetUserDataRequest(userInformation, _routineSuccess);
    }

    public void SetUserDataRequest(IPlayFabData playFabData, Action<UpdateUserDataResult> _routineSuccess)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = playFabData.GetDictionary()
        }, _routineSuccess,
        (_) => { OnDebugPlayFabFailure(_, "SetUserData"); });  
    }

    private void SetDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log(result.DataVersion);
    }
}
