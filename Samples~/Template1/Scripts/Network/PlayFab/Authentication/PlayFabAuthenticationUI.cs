using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using PlayFab.ClientModels;
using TMPro;
using System.Runtime.CompilerServices;

public class PlayFabAuthenticationUI : MonoBehaviour
{
    public string playFabID { get; set; }

    [Header("Components")]
    [SerializeField] PlayFabAuthenticationNetwork playFabAuthenticationNetwork;
    [SerializeField] UICanvasManager uiCanvasManager;
    [SerializeField] PlayFabPlayerDataNetwork playFabPlayerDataNetwork;

    [Header("Scene")]
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private string sceneName;

    [Header("Login")]
    [SerializeField] private InputField userID_Login;
    [SerializeField] private InputField userPassword_Login;

    [Header("VR Login")]
    [SerializeField] private TMP_InputField userID_Login_TMP;
    [SerializeField] private TMP_InputField userPassword_Login_TMP;

    [Header("Register")]
    [SerializeField] private InputField userID_Register;
    [SerializeField] private InputField userPassword_Register;
    [SerializeField] private InputField userPasswordCheck_Register;
    [SerializeField] private InputField userEmail_Register;
    [SerializeField] private InputField userName_Register;

    [Header("VR Register")]
    [SerializeField] private TMP_InputField userID_Register_TMP;
    [SerializeField] private TMP_InputField userPassword_Register_TMP;
    [SerializeField] private TMP_InputField userPasswordCheck_Register_TMP;
    [SerializeField] private TMP_InputField userEmail_Register_TMP;
    [SerializeField] private TMP_InputField userName_Register_TMP;

    [Header("UI Systems")]
    [SerializeField] private UIPopUp uiPopUp;
    [SerializeField] private UIPopUp uiLoading;

    private void Start()
    {
        if (XRSettings.enabled)
        {
            userID_Login.gameObject.SetActive(false);
            userPassword_Login.gameObject.SetActive(false);
            userID_Register.gameObject.SetActive(false);
            userPassword_Register.gameObject.SetActive(false);
            userPasswordCheck_Register.gameObject.SetActive(false);
            userEmail_Register.gameObject.SetActive(false);
            userName_Register.gameObject.SetActive(false);

            userID_Login_TMP.gameObject.SetActive(true);
            userPassword_Login_TMP.gameObject.SetActive(true);
            userID_Register_TMP.gameObject.SetActive(true);
            userPassword_Register_TMP.gameObject.SetActive(true);
            userPasswordCheck_Register_TMP.gameObject.SetActive(true);
            userEmail_Register_TMP.gameObject.SetActive(true);
            userName_Register_TMP.gameObject.SetActive(true);
        }
    }

    public void Login()
    {
        //uiLoading.OpenCanvas(LocalizeManager.instance.LocalizeScriptMessage("로그인 연결"),
        //    LocalizeManager.instance.LocalizeScriptMessage("로딩중..."));

        uiLoading.OpenCanvas("로그인 연걸", "로딩중...");

        string userID_LoginText;
        string userPassword_LoginText;

        if (XRSettings.enabled)
        {
            userID_LoginText = userID_Login_TMP.text;
            userPassword_LoginText = userPassword_Login_TMP.text;
        }
        else
        {
            userID_LoginText = userID_Login.text;
            userPassword_LoginText = userPassword_Login.text;
        }

        playFabAuthenticationNetwork.Login
        (
            new PlayFabUserAccount(userID_LoginText, userPassword_LoginText),
            (_emailverification, _ID, _displayName) =>
            {
                if (CheckEmailStatus(_emailverification))
                {
                    this.playFabID = _ID;
                    //playFabPlayerDataNetwork.GetUserCustomizeData(this.playFabID,
                    //    (_) => { PlayFabPlayerDataLoader.Instance.data.UserCustomize = _; playFabAddressableUI.Load(); });
                    playFabPlayerDataNetwork.GetUserInformationData(this.playFabID,
                        (_informationData) =>
                        {
                            playFabPlayerDataNetwork.GetUserCustomizeData(this.playFabID,
                                (_customizeData) =>
                                {
                                    PlayFabPlayerDataLoader.Instance.data.UserInformation = _informationData;
                                    PlayFabPlayerDataLoader.Instance.data.UserCustomize = _customizeData;
                                    PlayFabPlayerDataLoader.Instance.data.UserDisplayName = _displayName;

                                    CheckAndUpdateDisplayName
                                    (
                                        PlayFabPlayerDataLoader.Instance.data.UserInformation,
                                        PlayFabPlayerDataLoader.Instance.data.UserDisplayName,
                                        () =>
                                        {
                                            sceneLoader.LoadScene(sceneName);
                                        }
                                    );

                                    //playFabAddressableUI.Load();
                                });
                        });
                    return;
                }
                //OpenPopUp(AuthenticationError.EmailVerification, 
                //    LocalizeManager.instance.LocalizeScriptMessage("이메일 인증이 완료되지 않았습니다. 해당 계정의 인증을 완료하세요."));
                OpenPopUp(AuthenticationError.EmailVerification, "이메일 인증이 완료되지 않았습니다. 해당 계정의 인증을 완료하세요.");
            },
            (_) =>
            {

                Debug.Log(_.ToString());

                switch (_)
                {
                    case PlayFab.PlayFabErrorCode.InvalidParams:
                        OpenLoginError(AuthenticationError.InvalidInput, _);
                        break;
                    case PlayFab.PlayFabErrorCode.AccountNotFound:
                        OpenLoginError(AuthenticationError.InvalidInput, _);
                        break;
                    default:
                        OpenLoginError(AuthenticationError.None, _);
                        break;
                }

            }
        );
    }


    private UserInformation newUserInformation;
    private string newUserDisplayName;

    private void CheckAndUpdateDisplayName(UserInformation _userInformation, string _userDisplayName, Action _routineSuccess)
    {
        if (!string.IsNullOrEmpty(_userInformation.UserName) && _userInformation.UserName != "")
        {
            _routineSuccess();
            return;
        }

        newUserInformation = _userInformation;
        newUserInformation.UserName = _userDisplayName;
        newUserDisplayName = GenerateDisplayName(newUserInformation.UserName);
        playFabPlayerDataNetwork.SetUserInformationData(
            newUserInformation,
            (_) =>
            {
                playFabAuthenticationNetwork.UpdateDisplayName(newUserDisplayName, _routineSuccess);
            });

    }

    // For VRMode
    public void SetLoginInput(string _email, string _password)
    {
        if (XRSettings.enabled)
        {
            userID_Login_TMP.text = _email;
            userPassword_Login_TMP.text = _password;
        }
        else
        {
            userID_Login.text = _email;
            userPassword_Login.text = _password;
        }

        //userID_Login.text = _email;
        //userPassword_Login.text = _password;
    }

    private bool CheckEmailStatus(EmailVerificationStatus? _result)
    {
        switch (_result)
        {
            case EmailVerificationStatus.Confirmed:
                return true;

            case EmailVerificationStatus.Pending:
            case EmailVerificationStatus.Unverified:
            default:
                return false;

        }
    }


    public void Register()
    {
        //uiLoading.OpenCanvas(LocalizeManager.instance.LocalizeScriptMessage("회원가입 연결"),
        //    LocalizeManager.instance.LocalizeScriptMessage("로딩중..."));
        uiLoading.OpenCanvas("회원가입 연결", "로딩중...");


        string userID_RegisterText;
        string userPassword_RegisterText;
        string userEmail_RegisterText;
        string userName_RegisterText;

        if (XRSettings.enabled)
        {
            userID_RegisterText = userID_Register_TMP.text;
            userPassword_RegisterText = userPassword_Register_TMP.text;
            userEmail_RegisterText = userEmail_Register_TMP.text;
            userName_RegisterText = userName_Register_TMP.text;
        }
        else
        {
            userID_RegisterText = userID_Register.text;
            userPassword_RegisterText = userPassword_Register.text;
            userEmail_RegisterText = userEmail_Register.text;
            userName_RegisterText = userName_Register.text;
        }

        string errorCommnet = "";

        if (!CheckRegisterCondition(ref errorCommnet))
        {
            Debug.Log("Register Condition Error");
            OpenPopUp(AuthenticationError.InvalidInput, errorCommnet);
            return;
        }

        playFabAuthenticationNetwork.Register
        (
            new PlayFabUserRegister(userID_RegisterText, userPassword_RegisterText, userEmail_RegisterText, GenerateDisplayName(userName_RegisterText), new UserInformation("User Address", userName_RegisterText, UserType.Standard)),
            () =>
            {
                playFabPlayerDataNetwork.SetUserInformationData(new UserInformation("User Address", userName_RegisterText, UserType.Standard),
                (_) =>
                {
                    uiCanvasManager.OpenCanvas("Login");
                    uiLoading.CloseCanvas();
                    //uiPopUp.OpenCanvas(LocalizeManager.instance.LocalizeScriptMessage("회원가입 완료"),
                    //    LocalizeManager.instance.LocalizeScriptMessage("입학 등록을 완료하였습니다. 가입한 이메일을 확인하여 인증을 진행하세요."));
                    uiPopUp.OpenCanvas("회원가입 완료", "입학 등록을 완료하였습니다. 가입한 이메일을 확인하여 인증을 진행하세요.");

                });
            },
            (_) => { OpenRegisterError(AuthenticationError.NetworkConnection, _); }
        );
    }

    private string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private string GenerateDisplayName(string _userName)
    {
        return _userName + "#" + GenerateRandomCode(6);
    }

    private string GenerateRandomCode(int _codeLength)
    {
        string code = "";
        for (int i = 0; i < _codeLength; i++)
        {
            code = code + characters[UnityEngine.Random.Range(0, characters.Length)];
        }

        return code;
    }

    private bool CheckRegisterCondition(ref string errorComment)
    {
        Debug.Log("Register Condition");

        string userID_RegisterText;
        string userPassword_RegisterText;
        string userPasswordCheck_RegisterText;
        string userEmail_RegisterText;
        string userName_RegisterText;

        if (XRSettings.enabled)
        {
            userID_RegisterText = userID_Register_TMP.text;
            userPassword_RegisterText = userPassword_Register_TMP.text;
            userPasswordCheck_RegisterText = userPasswordCheck_Register_TMP.text;
            userEmail_RegisterText = userEmail_Register_TMP.text;
            userName_RegisterText = userName_Register_TMP.text;
        }
        else
        {
            userID_RegisterText = userID_Register.text;
            userPassword_RegisterText = userPassword_Register.text;
            userPasswordCheck_RegisterText = userPasswordCheck_Register.text;
            userEmail_RegisterText = userEmail_Register.text;
            userName_RegisterText = userName_Register.text;
        }

        // UserName length
        if (!CheckStringLength(userID_RegisterText, 3, 20))
        {
            //errorComment = LocalizeManager.instance.LocalizeScriptMessage("ID는 3자 이상 및 20자 이하로 작성해주세요.");
            errorComment = "ID는 3자 이상 및 20자 이하로 작성해주세요.";
            return false;
        }

        // Password length
        if (!CheckStringLength(userPassword_RegisterText, 6, 100))
        {
            //errorComment = LocalizeManager.instance.LocalizeScriptMessage("비밀번호는 6자 이상 및 100자 이하로 작성해주세요.");
            errorComment = "비밀번호는 6자 이상 및 100자 이하로 작성해주세요.";
            return false;
        }

        // Password Equal
        if (userPassword_RegisterText != userPasswordCheck_RegisterText)
        {
            //errorComment = LocalizeManager.instance.LocalizeScriptMessage("비밀번호와 비밀번호 확인이 동일하지 않습니다.");
            errorComment = "비밀번호와 비밀번호 확인이 동일하지 않습니다.";
            return false;
        }

        // DisplayName length
        if (!CheckStringLength(userName_RegisterText, 3, 25))
        {
            //errorComment = LocalizeManager.instance.LocalizeScriptMessage("성명은 3자 이상 및 25자 이하로 작성해주세요.");
            errorComment = "성명은 3자 이상 및 25자 이하로 작성해주세요.";
            return false;
        }

        // Email Address Valid
        if (!CheckEmailValid(userEmail_RegisterText))
        {
            //errorComment = LocalizeManager.instance.LocalizeScriptMessage("이메일을 올바르게 작성해주세요.");
            errorComment = "이메일을 올바르게 작성해주세요.";
            return false;
        }

        return true;

    }

    private bool CheckEmailValid(string _stringValue)
    {
        return (!string.IsNullOrEmpty(_stringValue)) && (_stringValue.Contains('@')) && (_stringValue.Contains('.'));
    }

    private bool CheckStringLength(string _stringValue, int _minLength, int _maxLength)
    {
        return (!string.IsNullOrEmpty(_stringValue) && (_stringValue.Length >= _minLength) && (_stringValue.Length <= _maxLength));
    }


    public void CheckIDRedundancy()
    {
        string userID_RegisterText;

        if (XRSettings.enabled)
        {
            userID_RegisterText = userID_Register_TMP.text;
        }
        else
        {
            userID_RegisterText = userID_Register.text;
        }

        playFabAuthenticationNetwork.CheckIDRedundancy(userID_RegisterText,
            () => { Debug.Log("Done"); },
            (_) => { });

    }

    private void OpenLoginError(AuthenticationError _error, PlayFab.PlayFabErrorCode _errorCode)
    {
        OpenPopUp(_error, GetLoginInstructionFromErrorCode(_errorCode));
    }

    private void OpenRegisterError(AuthenticationError _error, PlayFab.PlayFabErrorCode _errorCode)
    {
        OpenPopUp(_error, GetRegisterInstructionFromErrorCode(_errorCode));
    }

    #region Pop Up

    private void OpenPopUp(AuthenticationError _error, string _content)
    {
        uiLoading.CloseCanvas();

        uiPopUp.OpenCanvas(ConvertAuthenticationError(_error), _content);
    }


    private string ConvertAuthenticationError(AuthenticationError _authenticationError)
    {
        switch (_authenticationError)
        {
            case AuthenticationError.EmailVerification:
                //return LocalizeManager.instance.LocalizeScriptMessage("이메일 인증 오류");
                return "이메일 인증 오류";
            case AuthenticationError.InvalidInput:
                //return LocalizeManager.instance.LocalizeScriptMessage("입력 오류");
                return "입력 오류";
            case AuthenticationError.Mismatch:
                //return LocalizeManager.instance.LocalizeScriptMessage("이메일 및 비밀번호 오류");
                return "이메일 및 비밀번호 오류";
            case AuthenticationError.NetworkConnection:
                //return LocalizeManager.instance.LocalizeScriptMessage("네트워크 오류");
                return "네트워크 오류";
            default:
                //return LocalizeManager.instance.LocalizeScriptMessage("알 수 없는 오류");
                return "알 수 없는 오류";
        }
    }

    private string GetLoginInstructionFromErrorCode(PlayFab.PlayFabErrorCode errorCode)
    {
        switch (errorCode)
        {
            case PlayFab.PlayFabErrorCode.InvalidUsernameOrPassword:
                //return LocalizeManager.instance.LocalizeScriptMessage("ID와 비밀번호가 일치하지 않습니다. 입력을 확인해주세요.");
                return "ID와 비밀번호가 일치하지 않습니다. 입력을 확인해주세요.";
            case PlayFab.PlayFabErrorCode.AccountNotFound:
                //return LocalizeManager.instance.LocalizeScriptMessage("찾을 수 없는 ID입니다. 신규 입학 또는 입력을 확인해주세요.");
                return "찾을 수 없는 ID입니다. 신규 입학 또는 입력을 확인해주세요.";
            case PlayFab.PlayFabErrorCode.InvalidParams:
                //return LocalizeManager.instance.LocalizeScriptMessage("ID 및 비밀번호가 올바르게 작성되었는지 확인해주세요.");
                return "ID 및 비밀번호가 올바르게 작성되었는지 확인해주세요.";
            default:
                //return ConvertErrorCode(errorCode) + LocalizeManager.instance.LocalizeScriptMessage(". 관리자에게 문의하세요.");
                return ConvertErrorCode(errorCode) + ". 관리자에게 문의하세요.";
        }
    }

    private string GetRegisterInstructionFromErrorCode(PlayFab.PlayFabErrorCode errorCode)
    {
        switch (errorCode)
        {
            case PlayFab.PlayFabErrorCode.InvalidParams:
                //return LocalizeManager.instance.LocalizeScriptMessage("ID 또는 이메일이 올바르게 작성되었는지 확인해주세요.");
                return "ID 또는 이메일이 올바르게 작성되었는지 확인해주세요.";
            case PlayFab.PlayFabErrorCode.UsernameNotAvailable:
                //return LocalizeManager.instance.LocalizeScriptMessage("이미 사용중인 ID 입니다. 다른 ID로 작성해주세요.");
                return "이미 사용중인 ID 입니다. 다른 ID로 작성해주세요.";
            case PlayFab.PlayFabErrorCode.EmailAddressNotAvailable:
                //return LocalizeManager.instance.LocalizeScriptMessage("이미 가입된 이메일 입니다. 다른 이메일로 작성해주세요.");
                return "이미 가입된 이메일 입니다. 다른 이메일로 작성해주세요.";
            default:
                //return ConvertErrorCode(errorCode) + LocalizeManager.instance.LocalizeScriptMessage(". 관리자에게 문의하세요.");
                return ConvertErrorCode(errorCode) + ". 관리자에게 문의하세요.";
        }
    }

    private string ConvertErrorCode(PlayFab.PlayFabErrorCode errorCode)
    {
        //return LocalizeManager.instance.LocalizeScriptMessage("에러코드 <") + ((int)errorCode).ToString() + ">";
        return "에러코드 <" + ((int)errorCode).ToString() + ">";
    }

    #endregion
}
