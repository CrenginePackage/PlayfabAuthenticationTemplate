using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIChatPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text chatText;
    [SerializeField] private float showChatTime = 3f;
    [SerializeField] private PhotonView photonView;

    public bool chatInputActivated { get; set; } = false;
    private Coroutine popupCoroutine;

    private void OnEnable()
    {
        UIHandler.OnPlayerJoinedRoom += SubscribeToCommunity;
        UIHandler.OnPlayerLeftRoom += UnsubscribeToCommunity;
    }

    private void OnDisable()
    {
        UIHandler.OnPlayerJoinedRoom -= SubscribeToCommunity;
        UIHandler.OnPlayerLeftRoom -= UnsubscribeToCommunity;
    }

    public void ShowEmotionChat(string textValue)
    {
        Debug.Log(photonView.Owner.UserId + " TextValue : " + textValue);
        photonView.RPC("PublishMessage", RpcTarget.AllViaServer, new object[1] { textValue });
    }


    public void ShowChat(string textValue)
    {
        if (!chatInputActivated) return;

        Debug.Log(photonView.Owner.UserId + " TextValue : " + textValue);
        photonView.RPC("PublishMessage", RpcTarget.AllViaServer, new object[1] { textValue });
    }

    [PunRPC]
    private void PublishMessage(string _textValue)
    {
        canvasGroup.alpha = 1;
        chatText.text = _textValue;

        if (popupCoroutine != null) StopCoroutine(popupCoroutine);
        popupCoroutine = StartCoroutine(CloseChat());
    }

    private IEnumerator CloseChat()
    {
        yield return new WaitForSeconds(showChatTime);
        canvasGroup.alpha = 0;
    }

    #region Community

    private void SubscribeToCommunity(string _playerName)
    {
        photonView.RPC("AddUserToCommunity", RpcTarget.AllViaServer, new object[1] { _playerName });
    }

    [PunRPC]
    private void AddUserToCommunity(string _playerName)
    {
        UIHandler.PlayerAdded(_playerName);
    }

    private void UnsubscribeToCommunity(string _playerName)
    {
        photonView.RPC("RemoveUserToCommunity", RpcTarget.AllViaServer, new object[1] { _playerName });
    }


    [PunRPC]
    private void RemoveUserToCommunity(string _playerName)
    {
        UIHandler.PlayerRemoved(_playerName);
    }
    #endregion

}
