using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class PlayFabAddressableNetwork : PlayFabBaseNetwork
{

    #region Initialize
    public void Initialize()
    {
        StartCoroutine(InitializeRoutine());
    }

    private IEnumerator InitializeRoutine()
    {
        Addressables.ResourceManager.ResourceProviders.Add(new AssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageHashProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageAssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageJsonAssetProvider());

        yield return Addressables.InitializeAsync();
    }


    #endregion

    public void LoadScene(AssetReference sceneReference, Action<float> progressAction)
    {
        StartCoroutine(LoadSceneRoutine(sceneReference, progressAction));
    }

    private IEnumerator LoadSceneRoutine(AssetReference sceneReference, Action<float> progressAction)
    {
        yield return InitializeRoutine();

        var async = Addressables.LoadSceneAsync(sceneReference);
        async.Completed += (_) => { Debug.Log("Scene Load Complete : " + _.Result.Scene.name); };

        StartCoroutine(PercentRoutine(async, progressAction));

        yield return async;
    }

    private IEnumerator PercentRoutine(AsyncOperationHandle asyncOperationHandle, Action<float> progressAction)
    {
        while (!asyncOperationHandle.IsDone)
        {
            progressAction(asyncOperationHandle.PercentComplete);
            yield return null;
        }
    }



}
