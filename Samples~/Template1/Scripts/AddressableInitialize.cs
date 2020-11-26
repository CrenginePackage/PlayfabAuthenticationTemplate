using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

public class AddressableInitialize : PlayFabDataLoader<AddressableInitialize>
{
    public static bool isInitialized = false;
    public static bool InitCDN = false;

    // Start is called before the first frame update
    void Start()
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

        Debug.Log("Addressable Initialized.");

        isInitialized = true;
    }
}
