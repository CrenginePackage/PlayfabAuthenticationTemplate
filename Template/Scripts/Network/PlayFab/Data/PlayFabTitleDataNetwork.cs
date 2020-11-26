using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public abstract class PlayFabTitleDataNetwork<T> : PlayFabBaseNetwork
{
    public void GetTitleData(Action<List<T>> _routineSuccess)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest() { Keys = new List<string>() { GetKeyValue() } },
            (_) => { GetTitleDataSuccess(_, _routineSuccess); },
            (_) => { OnDebugPlayFabFailure(_, "GetTitleData<" + typeof(T).ToString() + ">"); });
    }

    protected void GetTitleDataSuccess(GetTitleDataResult result, Action<List<T>> _routineSuccess)
    {
        _routineSuccess(JsonHelper.FromJson<T>(fixJson(result.Data[GetKeyValue()])).ToList());
    }

    protected abstract string GetKeyValue();

    protected string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
}
