
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class FloatCallboack : UnityEvent<float>
{

}

public class PlayFabAddressableUI : MonoBehaviour
{
    [SerializeField] private PlayFabAddressableNetwork playFabAddressableNetwork;
    [SerializeField] private AssetReference sceneReference;

    public UnityEvent OnAddressableLoadStart;
    public FloatCallboack OnAddressableLoading;

    public void Initialize()
    {
        playFabAddressableNetwork.Initialize();
    }

    public void Load()
    {
        OnAddressableLoadStart.Invoke();
        playFabAddressableNetwork.LoadScene(sceneReference, (_) => { OnAddressableLoading.Invoke(_); } );
    }

}
