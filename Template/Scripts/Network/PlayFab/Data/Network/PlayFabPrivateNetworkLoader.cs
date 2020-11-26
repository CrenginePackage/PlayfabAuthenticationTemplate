using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabPrivateNetworkLoader : PlayFabDataLoader<PlayFabPrivateNetworkLoader>
{
    [SerializeField] private PlayFabPrivateNetwork playFabPrivateNetwork;

    public PrivateNetwork data { get; set; } = new PrivateNetwork();

    public void CheckPrivateNetwork(string _privateNetworkValue, Action<PrivateNetwork> _routineSuccess)
    {
        playFabPrivateNetwork.CheckPrivateNetwork(_privateNetworkValue, _routineSuccess);
    }

}
