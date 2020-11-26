using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabPrivateNetwork : PlayFabBaseNetwork
{
    public void CheckPrivateNetwork(string _privateNetworkCode, Action<PrivateNetwork> _routineSuccess)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest()
        {
            Keys = new List<string>() { "PrivateNetwork" }
        },
        (_) => { GetPrivateNetworkSuccess(_, "PrivateNetwork", _privateNetworkCode, _routineSuccess); },
        (_) => { OnDebugPlayFabFailure(_, "GetTitleData<" + typeof(PrivateNetwork).ToString() + ">"); _routineSuccess(new PrivateNetwork()); });
    }

    private void GetPrivateNetworkSuccess(GetTitleDataResult result, string _datakey, string _privateNetworkCode, Action<PrivateNetwork> _routineSuccess)
    {
        PrivateNetwork privateNetwork = new PrivateNetwork();

        if (result.Data == null || !result.Data.ContainsKey(_datakey))
        {
            Debug.LogError("Cannot Get PrivateNetwork. Returning Empty");
        }
        else
        {
            privateNetwork = JsonConvert.DeserializeObject<List<PrivateNetwork>>(result.Data[_datakey]).Find((_ => _.PrivateNetworkAddress == _privateNetworkCode));
        }
        _routineSuccess(privateNetwork);
    }

    //public void GetPrivateNetwork(string _privateNetworkCode, Action<List<PrivateNetwork>> _routineSuccess)
    //{
    //    PlayFabClientAPI.GetTitleData(new GetTitleDataRequest()
    //    {
    //        Keys = new List<string>() { "PrivateNetwork" }
    //    },
    //    (_) => { GetPrivateNetworkSuccess(_, "PrivateNetwork", _routineSuccess); },
    //    (_) => { OnDebugPlayFabFailure(_, "GetTitleData<" + typeof(PrivateNetwork).ToString() + ">"); });
    //}

}
