using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerGenerator : MonoBehaviour
{
    public static PhotonPlayerGenerator Instance;

    public Transform spawnLocation;
    [SerializeField] private GameObject photonNetworkPlayer;

    [SerializeField] private GameObject Player_PC;
    [SerializeField] private GameObject Player_PC_Capture;
    [SerializeField] private GameObject Player_Mobile;
    [SerializeField] private GameObject Player_VR;

    private DefaultPool defaultPool;

    private void OnEnable()
    {
        if (PhotonPlayerGenerator.Instance == null)
            PhotonPlayerGenerator.Instance = this;
    }

    private void Start()
    {
        defaultPool = (DefaultPool)PhotonNetwork.PrefabPool;

        SetNetworkPrefabPool(defaultPool);
        SetPlayerPrefabPool(defaultPool);

        PhotonNetwork.Instantiate(
            photonNetworkPlayer.name,
            Vector3.zero,
            Quaternion.identity,
            0);

        //PhotonNetwork.Instantiate(
        //    Path.Combine("PhotonPrefabs", networkPaleyrName),
        //    Vector3.zero,
        //    Quaternion.identity,
        //    0);
    }

    private void SetNetworkPrefabPool(DefaultPool pool)
    {
        //DefaultPool pool = (DefaultPool)PhotonNetwork.PrefabPool;
        pool.ResourceCache.Add(photonNetworkPlayer.name, photonNetworkPlayer);
    }

    private void SetPlayerPrefabPool(DefaultPool pool)
    {
        //DefaultPool pool = (DefaultPool)PhotonNetwork.PrefabPool;

        pool.ResourceCache.Add(Player_PC.name, Player_PC);
        //pool.ResourceCache.Add(Player_PC_Capture.name, Player_PC_Capture);
        pool.ResourceCache.Add(Player_Mobile.name, Player_Mobile);
        pool.ResourceCache.Add(Player_VR.name, Player_VR);
    }


}
