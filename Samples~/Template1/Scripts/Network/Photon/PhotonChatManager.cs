using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Chat;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    //[SerializeField] private PhotonView photonView;
    [SerializeField] private UIChatPopup uiChatPopup;
    [SerializeField] private Text chatText;
    [SerializeField] private InputField chatInputField;
    [SerializeField] private TMP_InputField vrChatInputField;


    private ChatClient chatClient;
    private string chatChannel;

    private bool enableChat;
    private int chatHistoryLength;


    void Update()
    {
        if (!enableChat) return;

        chatClient.Service();

        OnEnterSend();
    }

    public void OnEmotionSend(string _emotionMessage)
    {
        this.SendChatMessage(_emotionMessage);
        uiChatPopup.ShowEmotionChat(_emotionMessage);
    }

    public void OnEnterSend()
    {
        if (!enableChat) return;

        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        //{
        //    if (this.chatInputField != null && !string.IsNullOrEmpty(this.chatInputField.text))
        //    {
        //        this.SendChatMessage(this.chatInputField.text);
        //        uiChatPopup.ShowChat(this.chatInputField.text);
        //        //photonView.RPC("ShowPopupMessage", RpcTarget.AllBufferedViaServer, new object[1] { this.chatInputField.text });
        //        this.chatInputField.text = "";
        //        this.chatInputField.ActivateInputField();
        //        this.chatInputField.Select();
        //    }
        //}
    }

    public void OnClickSend()
    {
        if (this.chatInputField != null && !string.IsNullOrEmpty(GetInputFieldText()))
        {
            this.SendChatMessage(GetInputFieldText());
            uiChatPopup.ShowChat(GetInputFieldText());
            //photonView.RPC("ShowPopupMessage", RpcTarget.AllBufferedViaServer, new object[1] { this.chatInputField.text });
            chatInputField.text = "";
            vrChatInputField.text = "";
        }
    }

    private string GetInputFieldText()
    {
        if (XRSettings.enabled)
        {
            return vrChatInputField.text;
        }

        return chatInputField.text;
    }

    //[PunRPC]
    //private void ShowPopupMessage(string message)
    //{
    //    uiChatPopup.ShowChat(message);
    //}

    public void Connect(string _userName, string _chatChannel)
    {
        enableChat = true;
   
        chatChannel = _chatChannel;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(_userName));

        Debug.Log("Connecting as: " + _userName);
    }


    public void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }

        this.chatClient.PublishMessage(chatChannel, inputLine);
        //if ("test".Equals(inputLine))
        //{
        //    if (this.TestLength != this.testBytes.Length)
        //    {
        //        this.testBytes = new byte[this.TestLength];
        //    }

        //    this.chatClient.SendPrivateMessage(this.chatClient.AuthValues.UserId, this.testBytes, true);
        //}


        //bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(this.selectedChannelName);
        //string privateChatTarget = string.Empty;
        //if (doingPrivateChat)
        //{
        //    // the channel name for a private conversation is (on the client!!) always composed of both user's IDs: "this:remote"
        //    // so the remote ID is simple to figure out

        //    string[] splitNames = this.selectedChannelName.Split(new char[] { ':' });
        //    privateChatTarget = splitNames[1];
        //}
        //UnityEngine.Debug.Log("selectedChannelName: " + selectedChannelName + " doingPrivateChat: " + doingPrivateChat + " privateChatTarget: " + privateChatTarget);


        //if (inputLine[0].Equals('\\'))
        //{
        //    string[] tokens = inputLine.Split(new char[] { ' ' }, 2);
        //    if (tokens[0].Equals("\\help"))
        //    {
        //        this.PostHelpToCurrentChannel();
        //    }
        //    if (tokens[0].Equals("\\state"))
        //    {
        //        int newState = 0;


        //        List<string> messages = new List<string>();
        //        messages.Add("i am state " + newState);
        //        string[] subtokens = tokens[1].Split(new char[] { ' ', ',' });

        //        if (subtokens.Length > 0)
        //        {
        //            newState = int.Parse(subtokens[0]);
        //        }

        //        if (subtokens.Length > 1)
        //        {
        //            messages.Add(subtokens[1]);
        //        }

        //        this.chatClient.SetOnlineStatus(newState, messages.ToArray()); // this is how you set your own state and (any) message
        //    }
        //    else if ((tokens[0].Equals("\\subscribe") || tokens[0].Equals("\\s")) && !string.IsNullOrEmpty(tokens[1]))
        //    {
        //        this.chatClient.Subscribe(tokens[1].Split(new char[] { ' ', ',' }));
        //    }
        //    else if ((tokens[0].Equals("\\unsubscribe") || tokens[0].Equals("\\u")) && !string.IsNullOrEmpty(tokens[1]))
        //    {
        //        this.chatClient.Unsubscribe(tokens[1].Split(new char[] { ' ', ',' }));
        //    }
        //    else if (tokens[0].Equals("\\clear"))
        //    {
        //        if (doingPrivateChat)
        //        {
        //            this.chatClient.PrivateChannels.Remove(this.selectedChannelName);
        //        }
        //        else
        //        {
        //            ChatChannel channel;
        //            if (this.chatClient.TryGetChannel(this.selectedChannelName, doingPrivateChat, out channel))
        //            {
        //                channel.ClearMessages();
        //            }
        //        }
        //    }
        //    else if (tokens[0].Equals("\\msg") && !string.IsNullOrEmpty(tokens[1]))
        //    {
        //        string[] subtokens = tokens[1].Split(new char[] { ' ', ',' }, 2);
        //        if (subtokens.Length < 2) return;

        //        string targetUser = subtokens[0];
        //        string message = subtokens[1];
        //        this.chatClient.SendPrivateMessage(targetUser, message);
        //    }
        //    else if ((tokens[0].Equals("\\join") || tokens[0].Equals("\\j")) && !string.IsNullOrEmpty(tokens[1]))
        //    {
        //        string[] subtokens = tokens[1].Split(new char[] { ' ', ',' }, 2);

        //        // If we are already subscribed to the channel we directly switch to it, otherwise we subscribe to it first and then switch to it implicitly
        //        if (this.channelToggles.ContainsKey(subtokens[0]))
        //        {
        //            this.ShowChannel(subtokens[0]);
        //        }
        //        else
        //        {
        //            this.chatClient.Subscribe(new string[] { subtokens[0] });
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("The command '" + tokens[0] + "' is invalid.");
        //    }
        //}
        //else
        //{
        //    if (doingPrivateChat)
        //    {
        //        this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
        //    }
        //    else
        //    {
        //        this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
        //    }
        //}
    }



    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnChannelPropertiesChanged(string channel, string senderUserId, Dictionary<object, object> properties)
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("Chat State : " + state);
    }

    public void OnConnected()
    {
        this.chatClient.Subscribe(chatChannel, chatHistoryLength);

        //if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
        //{
        //    this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
        //}

        //this.ConnectingLabel.SetActive(false);

        //this.UserIdText.text = "Connected as " + this.UserName;

        //this.ChatPanel.gameObject.SetActive(true);

        //if (this.FriendsList != null && this.FriendsList.Length > 0)
        //{
        //    this.chatClient.AddFriends(this.FriendsList); // Add some users to the server-list to get their status updates

        //    // add to the UI as well
        //    foreach (string _friend in this.FriendsList)
        //    {
        //        if (this.FriendListUiItemtoInstantiate != null && _friend != this.UserName)
        //        {
        //            this.InstantiateFriendButton(_friend);
        //        }

        //    }

        //}

        //if (this.FriendListUiItemtoInstantiate != null)
        //{
        //    this.FriendListUiItemtoInstantiate.SetActive(false);
        //}


        this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnErrorInfo(string channel, string error, object data)
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(this.chatChannel))
        {
            // update text
            this.ShowChannel(this.chatChannel);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribled");

        //uiChatPopup.SubscribeToCommunity(this.chatClient.UserId);
        this.chatClient.PublishMessage(chatChannel, "님이 접속했습니다.");
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("Subscribled");

        //uiChatPopup.UnsubscribeToCommunity(this.chatClient.UserId);
        this.chatClient.PublishMessage(chatChannel, "님이 퇴장했습니다.");
    }

    public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        chatChannel = channelName;
        chatText.text = channel.ToStringMessages(); 
    }

}
