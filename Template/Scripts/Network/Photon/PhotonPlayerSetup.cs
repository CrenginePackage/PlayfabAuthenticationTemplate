using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PhotonPlayerSetup : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected PlayerSetup playerSetup;
    [SerializeField] protected GameObject[] systemObjects;
    [SerializeField] protected GameObject cameraObject;

    [Header("Photon Component")]
    [SerializeField] protected PhotonChatManager photonChatManager;

    protected PlayFabPlayerData playerData;

    public void Init(PlayFabPlayerData _playerData)
    {
        playerData = _playerData;
    }

    protected void Start()
    {
        photonView.RPC("NetworkSetup", RpcTarget.AllViaServer);

        if (photonView.IsMine)
        {
            PhotonSetup();
        }
    }

    protected virtual void PhotonSetup()
    {
        photonChatManager.Connect(playerData.UserInformation.UserName, "General");
        
    }

    [PunRPC]
    public void NetworkSetup()
    {
        for (int i = 0; i < systemObjects.Length; i++)
        {
            systemObjects[i].SetActive(photonView.IsMine);
        }

        cameraObject.SetActive(photonView.IsMine);

        if (photonView.IsMine)
        {
            photonView.RPC("GetPlayerCustom", RpcTarget.AllViaServer,new object[6] {
                playerData.UserCustomize.GetUserCustomize(),
                playerData.UserInformation.UserName,
                playerData.UserInformation.UserAddress,
                (int) playerData.UserInformation.UserType,
                (int) playerData.UserSpecialAvatar.SpecialAvatarSelected,
                playerData.UserDisplayName});
        }
    }

    [PunRPC]
    public void GetPlayerCustom(string _userCustomizeValue, string _userName, string _userAddress, int _userType, int _userSpecialAvatar, string _userDisplayName)
    {
        playerData = new PlayFabPlayerData(new UserCustomize(_userCustomizeValue), new UserInformation(_userAddress, _userName, (UserType)_userType), new UserSpecialAvatar(0, (SpecialAvatarNumber)_userSpecialAvatar, new List<int>()), _userDisplayName);
        playerSetup.Init(playerData, !photonView.IsMine);
    }

    
    
}
