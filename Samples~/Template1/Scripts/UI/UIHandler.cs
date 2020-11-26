using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIHandler
{
    public delegate void VoidDelegate();
    public delegate void IntDelegate(int value);
    public delegate void StringDelegate(string value);
    public delegate void TransformDelegate(Vector3 _position, Quaternion _rotation);
    //public delegate void ArtifactDataTransformDelegate(ArtifactInformation _information, Transform _location);
    //public delegate void ArtifactDataDelegate(ArtifactInformation _information);
    //public delegate void PlaceInformationTransformDelegate(PlaceInformation _information, Transform _location);
    //public delegate void PlaceInformationDelegate(PlaceInformation _information);
    public delegate void PlayerDataDelegate(PlayFabPlayerData _playerData);

    public static event VoidDelegate OnUIOpened;
    public static event VoidDelegate OnUIClosed;

    public static event VoidDelegate OnChatInputEnabled;
    public static event VoidDelegate OnChatInputDisabled;

    public static event VoidDelegate OnMapButtonClicked;
    public static event TransformDelegate OnTeleportButtonClicked;

    //public static event ArtifactDataTransformDelegate OnArtifactDisplayClicked;
    //public static event PlaceInformationDelegate OnPlaceMapClicked;
    //public static event PlaceInformationTransformDelegate OnPlaceDisplayClicked;
    public static event PlayerDataDelegate OnPlayerDisplayClicked;

    public static event StringDelegate OnPlayerAdded;
    public static event StringDelegate OnPlayerRemoved;

    public static event StringDelegate OnPlayerJoinedRoom;
    public static event StringDelegate OnPlayerLeftRoom;

    //public static void ArtifactDisplayClicked(ArtifactInformation _artifactInformation, Transform _uiLocation)
    //{
    //    if (OnArtifactDisplayClicked != null)
    //    {
    //        OnArtifactDisplayClicked(_artifactInformation, _uiLocation);
    //    }
    //}

    //public static void PlaceDisplayClicked(PlaceInformation _placeInformation, Transform _uiLocation)
    //{
    //    if (OnPlaceDisplayClicked != null)
    //    {
    //        OnPlaceDisplayClicked(_placeInformation, _uiLocation);
    //    }
    //}

    //public static void PlaceMapClicked(PlaceInformation _placeInformation)
    //{
    //    if (OnPlaceMapClicked != null)
    //    {
    //        OnPlaceMapClicked(_placeInformation);
    //    }
    //}

    public static void PlayerDisplayClicked(PlayFabPlayerData _playerData)
    {
        if (OnPlayerDisplayClicked != null)
        {
            OnPlayerDisplayClicked(_playerData);
        }
    }

    public static void MapButtonClicked()
    {
        if (OnMapButtonClicked != null)
        {
            OnMapButtonClicked();
        }
    }

    public static void TeleportButtonClicked(Vector3 _teleportLocation, Quaternion _teleportRotation)
    {
        if (OnTeleportButtonClicked != null)
        {
            OnTeleportButtonClicked(_teleportLocation, _teleportRotation);
        }
    }

    public static void UIOpened()
    {
        if (OnUIOpened != null)
        {
            OnUIOpened();
        }
    }

    public static void UIClosed()
    {
        if (OnUIClosed != null)
        {
            OnUIClosed();
        }
    }

    public static void ChatInputEnabled()
    {
        if (OnChatInputEnabled != null)
        {
            OnChatInputEnabled();
        }
    }

    public static void ChatInputDisabled()
    {
        if (OnChatInputDisabled != null)
        {
            OnChatInputDisabled();
        }
    }

    public static void PlayerAdded(string _userName)
    {
        if (OnPlayerAdded != null)
        {
            OnPlayerAdded(_userName);
        }
    }

    public static void PlayerRemoved(string _userName)
    {
        if (OnPlayerRemoved != null)
        {
            OnPlayerRemoved(_userName);
        }
    }

    public static void PlayerJoinedRoom(string _userName)
    {
        if (OnPlayerJoinedRoom != null)
        {
            OnPlayerJoinedRoom(_userName);
        }
    }

    public static void PlayerLeftRoom(string _userName)
    {
        if (OnPlayerLeftRoom != null)
        {
            OnPlayerLeftRoom(_userName);
        }
    }

}
