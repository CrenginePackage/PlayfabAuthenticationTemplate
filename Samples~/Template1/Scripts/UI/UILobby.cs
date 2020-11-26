using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    //[SerializeField] private SceneLoader sceneLoader;
    //[SerializeField] private string sceneName;
    [SerializeField] private PhotonLauncher photonLauncher;
    //[SerializeField] PlayFabAddressableUI addressableUI;
    [SerializeField] private UICanvasManager uiCanvasManager;
    [SerializeField] private ApplicationExit applicationExit;
    [SerializeField] private Text loadingText;

    [Header("PC Data")]
    [SerializeField] private string sceneName_PC;
    [SerializeField] private string loadingComments_PC;

    [Header("Mobile Data")]
    [SerializeField] private string sceneName_Mobile;
    [SerializeField] private string loadingComments_Mobile;

    [Header("VR Data")]
    [SerializeField] private GameObject loadingDescription_VR;
    [SerializeField] private GameObject TextObejct;

    public void Connect()
    {
        uiCanvasManager.OpenCanvas("Loading");
        SaveUserData(() => {
            //    sceneLoader.LoadScene(sceneName);

#if UNITY_ANDROID
            ConnectAsMobile();
#else
            if (!XRSettings.enabled)
                ConnectAsPC();
            else
                ConnectAsVR();
            // photonLauncher.Connect(sceneName_PC);
#endif
        });

        //PlayFabPlayerDataLoader.Instance.SaveUserCustomize(() => { playFabAddressableUI.Load(); });
    }

    private void ConnectAsPC()
    {
        photonLauncher.Connect(sceneName_PC, PlayFabPrivateNetworkLoader.Instance.data);
        //loadingText.text = loadingComments_PC;

        //loadingText.text =
        //    LocalizeManager.instance.LocalizeScriptMessage("이동 후 F1을 눌러 조작법을 확인하세요.");
        loadingText.text = "로딩 텍스트 입력";

        // Template start
        // This is template example connection, without photon
        //sceneLoader.LoadScene(sceneName);
        // Template end
    }

    private void ConnectAsMobile()
    {
        //photonLauncher.Connect(sceneName_Mobile, PlayFabPrivateNetworkLoader.Instance.data);
        //loadingText.text = loadingComments_Mobile;
    }

    private void ConnectAsVR()
    {
        //loadingDescription_VR.SetActive(true);
        //TextObejct.SetActive(false);

        loadingText.text = "로딩 텍스트 입력";
        photonLauncher.Connect(sceneName_PC, PlayFabPrivateNetworkLoader.Instance.data);
    }
    

    public void Logout()
    {
        SaveUserData(() => { applicationExit.Exit(); });
        //PlayFabPlayerDataLoader.Instance.SaveUserCustomize(() => { applicationExit.Exit(); });
    }

    private void SaveUserData(System.Action _routineSuccess)
    {
        PlayFabPlayerDataLoader.Instance.SaveUserCustomize(
            () => 
            {
                PlayFabPlayerDataLoader.Instance.SaveUserInformation(
                    () =>
                    {
                        _routineSuccess();
                        //PlayFabPlayerDataLoader.Instance.SaveUserSpecialAvatar(
                        //    () =>
                        //    {
                        //        _routineSuccess();
                        //    });
                        //playFabAddressableUI.Load();
                    });
            });
    }
}
