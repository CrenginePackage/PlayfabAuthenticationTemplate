using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayFabTitleDataLoader<T1,T2> :PlayFabDataLoader<T1> where T1 : Component
{
    public List<T2> data { get; set; } = new List<T2>();
    public bool dataLoaded { get; set; } = false;

    [SerializeField] protected PlayFabTitleDataNetwork<T2> playFabTitleDataNetwork;

    protected virtual void Start()
    {
        CreateNetwork();
        GetTitleData();
    }

    private void CreateNetwork()
    {
        Debug.Log("Create Network");
        GameObject networkObject = new GameObject(typeof(T2).ToString() + "Network");
        networkObject.transform.SetParent(this.transform);
        playFabTitleDataNetwork = CreateNetworkComponent(networkObject);
    }

    protected abstract PlayFabTitleDataNetwork<T2> CreateNetworkComponent(GameObject networkObject);

    protected void GetTitleData()
    {
        Debug.Log("Get Title Data");
        playFabTitleDataNetwork.GetTitleData(
        (_) => 
        {
            data = _;
            dataLoaded = true;
            Debug.Log("Get Title Data Done");

            SetTitleInformation(data);
        });
    }

    public void InvokeGetTitleData()
    {
        GetTitleData();
    }

    protected abstract void SetTitleInformation(List<T2> _data);
}
