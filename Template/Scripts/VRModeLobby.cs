using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class VRModeLobby : VRModeSetting
{
    [Header("Login Information")]
    [SerializeField] private UILobby uiLobby;

    protected override void AutoWork()
    {
        uiLobby.Connect();
    }

}
