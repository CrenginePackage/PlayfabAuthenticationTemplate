using System.IO;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PhotonNetworkPlayer : MonoBehaviour
{
    [SerializeField] private GameObject Player_PC;
    [SerializeField] private GameObject Player_Mobile;
    [SerializeField] private GameObject Player_VR;

    [SerializeField] private PhotonView photonView;

    void Start()
    {
        //SetPlayerPrefabPool();

        if (photonView.IsMine)
        {

#if UNITY_ANDROID

            PhotonNetwork.Instantiate(
                Player_Mobile.name,
                PhotonPlayerGenerator.Instance.spawnLocation.position,
                //PhotonPlayerSetup.Instance.spawnLocation.rotation).GetComponent<PlayerSetup>().Init(PlayFabPlayerDataLoader.Instance.data.UserCustomize);
                PhotonPlayerGenerator.Instance.spawnLocation.rotation).GetComponent<PhotonPlayerSetup>().Init(PlayFabPlayerDataLoader.Instance.data);

            //PhotonNetwork.Instantiate(
            //    Path.Combine("PhotonPrefabs", avatarName_Mobile),
            //    ExhibitionSetupController.Instance.spawnLocation.position,
            //    ExhibitionSetupController.Instance.spawnLocation.rotation, 0);
#else
            if (XRSettings.isDeviceActive)
            {
                PhotonNetwork.Instantiate(
                    Player_VR.name, 
                    PhotonPlayerGenerator.Instance.spawnLocation.position,
                    //PhotonPlayerGenerator.Instance.spawnLocation.rotation).GetComponent<PlayerSetup>().Init(PlayFabPlayerDataLoader.Instance.data.UserCustomize);
                    PhotonPlayerGenerator.Instance.spawnLocation.rotation).GetComponent<PhotonPlayerSetup>().Init(PlayFabPlayerDataLoader.Instance.data);

                //PhotonNetwork.Instantiate(
                //    Path.Combine("PhotonPrefabs", avatarName_VR),
                //    ExhibitionSetupController.Instance.spawnLocation.position,
                //    ExhibitionSetupController.Instance.spawnLocation.rotation, 0);
            }
            else
            {
                PhotonNetwork.Instantiate(
                    Player_PC.name, 
                    PhotonPlayerGenerator.Instance.spawnLocation.position,
                    ///PhotonPlayerGenerator.Instance.spawnLocation.rotation).GetComponent<PlayerSetup>().Init(PlayFabPlayerDataLoader.Instance.data.UserCustomize);
                    PhotonPlayerGenerator.Instance.spawnLocation.rotation).GetComponent<PhotonPlayerSetup>().Init(PlayFabPlayerDataLoader.Instance.data);

                //PhotonNetwork.Instantiate(
                //    Path.Combine("PhotonPrefabs", avatarName_PC),
                //    ExhibitionSetupController.Instance.spawnLocation.position,
                //    ExhibitionSetupController.Instance.spawnLocation.rotation, 0);
            }
#endif
        }
    }
}
