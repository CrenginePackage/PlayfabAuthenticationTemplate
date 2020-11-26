using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private Button loginButton;
    [SerializeField] private int defaultRoomSize = 20;
    [SerializeField] private string defaultRoomName = "default";
    public int roomIndex = 1;

    //private string _currentRoomName;
    private string currentSceneName;
    private string currentRoomName;
    public int currentRoomSize = 10;

    public void Connect(string _sceneName, PrivateNetwork privateNetwork)
    {
        PhotonNetwork.NickName = PlayFabPlayerDataLoader.Instance.data.UserDisplayName;
        currentSceneName = _sceneName;
        currentRoomName = privateNetwork.PrivateNetworkAddress;

        if (string.IsNullOrEmpty(currentRoomName) || currentRoomName == "")
        {
            currentRoomName = defaultRoomName;
            currentRoomSize = defaultRoomSize;
        }
        else
        {
            currentRoomSize = privateNetwork.PrivateNetworkMaxPlayer;
        }

        loginButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //loginButton.interactable = true;
        Debug.Log("Joining Room : " + currentRoomName);
        PhotonNetwork.JoinRoom(currentRoomName);
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log("Joined room ; " + _currentRoomName);
        PhotonNetwork.LoadLevel(roomIndex);
        sceneLoader.LoadScene(currentSceneName);
        //SceneManager.LoadScene(_currentRoomName, LoadSceneMode.Additive);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + ": Failed to join a room. Start creating one as, " + currentRoomName);
        CreateRoom(currentRoomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + ": Failed to create a room, " + message);
        CreateRoom(currentRoomName);
    }

    public void CreateRoom(string _roomName)
    {
        Debug.Log("Creating room..." + _roomName);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)currentRoomSize };
        PhotonNetwork.CreateRoom(_roomName, roomOps);
        Debug.Log("Room : " + _roomName);
    }
}
