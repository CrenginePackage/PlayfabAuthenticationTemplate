using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class VRModeAuthentication : VRModeSetting
{
    [Header("Login Information")]
    [SerializeField] private string SampleEmail;
    [SerializeField] private string SamplePassword;

    [Header("Authentication")]
    [SerializeField] PlayFabAuthenticationUI playFabAuthenticationUI;

    protected override void AutoWork()
    {
        playFabAuthenticationUI.SetLoginInput(SampleEmail, SamplePassword);
        playFabAuthenticationUI.Login();
    }
}
