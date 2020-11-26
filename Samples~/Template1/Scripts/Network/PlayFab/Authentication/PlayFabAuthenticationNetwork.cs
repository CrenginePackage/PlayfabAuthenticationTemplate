using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabAuthenticationNetwork : PlayFabBaseNetwork
{
    public void Login(PlayFabUserAccount _userAccount, Action<EmailVerificationStatus?, string, string> _routineSuccess, Action<PlayFabErrorCode> _routineFailure)
    {
        // Send Login Request
        SendLoginRequest
        (
            _userAccount,

            // Login Success => Check Email Status
            (loginResult) =>
            {
                OnDebugRequestSuccess("SendLoginRequest");

                CheckEmailStatusRequest
                (
                    loginResult.PlayFabId,

                    // CheckEmailStatus Success
                    (checkEmailResult) =>
                    {
                        OnDebugRequestSuccess("CheckEmailStatusRequest");
                        _routineSuccess(checkEmailResult.PlayerProfile.ContactEmailAddresses[0].VerificationStatus, loginResult.PlayFabId, checkEmailResult.PlayerProfile.DisplayName);
                    },

                    // CheckEmailStatus Failure
                    (checkEmailFailure) =>
                    {
                        OnDebugPlayFabFailure(checkEmailFailure, "CheckEmailStatusRequest");
                        _routineFailure(checkEmailFailure.Error);
                    }
                );
            },

            // Login Failure
            (loginFailure) =>
            {
                OnDebugPlayFabFailure(loginFailure, "SendRegisterRequest");
                _routineFailure(loginFailure.Error);
            }
        );
    }

    public void Register(PlayFabUserRegister _userRegister, Action _routineSuccesss, Action<PlayFabErrorCode> _routineFailure)
    {
        // Send Register Request
        SendRegisterRequest
        (
            _userRegister,

            //  Register Success -> Get Register Email Request
            (registerResult) =>
            {
                OnDebugRequestSuccess("SendRegisterRequest");

                GetRegisterEmailReqeust
                (
                    registerResult.PlayFabId,

                    // GetEmail Success -> Send Email Verification Request
                    (getEmailResult) =>
                    {
                        OnDebugRequestSuccess("GetRegisterEmailReqeust");

                        SendEmailVerificationRequest
                        (
                            getEmailResult.AccountInfo.PrivateInfo.Email,
                            
                            // SendEamil Success
                            (sendEmailResult) =>
                            {
                                OnDebugRequestSuccess("SendEmailVerificationRequest");
                                _routineSuccesss();
                            },

                            // SendEmail Failure
                            (sendEmailFailure) =>
                            {
                                OnDebugPlayFabFailure(sendEmailFailure, "SendEmailVerificationRequest");
                                _routineFailure(sendEmailFailure.Error);
                            }
                        );
                    },

                    // GetEmail Failure
                    (getEmailFailure) =>
                    {
                        OnDebugPlayFabFailure(getEmailFailure, "GetRegisterEmailReqeust");
                        _routineFailure(getEmailFailure.Error);
                    }
                );
            },

            // Register Failure
            (registerFailure) =>
            {
                OnDebugPlayFabFailure(registerFailure, "SendLoginRequest");
                _routineFailure(registerFailure.Error);
            }
        );
    }

    public void UpdateDisplayName(string _displayName, Action _routineSuccess)
    {
        SendUpdateDisplayNameRequest
        (
            _displayName,
            (UpdateDisplayNameSuccess) =>
            {
                OnDebugRequestSuccess("UpdateDisplayName");
                _routineSuccess();
            },
            (UpdateDisplayNameFailure) =>
            {
                OnDebugPlayFabFailure(UpdateDisplayNameFailure,"UpdateDisplayName");
            }
        );
    }

    private void SendUpdateDisplayNameRequest(string _displayName, Action<UpdateUserTitleDisplayNameResult> _routineSuccess ,Action<PlayFabError> _routineFailure)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = _displayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, _routineSuccess, _routineFailure);
        OnDebugRequestSend("SendUpdateDisplayNameRequest");
    }

    public void CheckIDRedundancy(string _IDValue, Action _routineSuccesss, Action<string> _routineFailure)
    {
        CheckIDRedundancyRequest
        (
            _IDValue, 

            (checkIDRedundancyResult) => 
            {
                OnDebugRequestSuccess("CheckIDRedundancyRequest");
                Debug.Log(checkIDRedundancyResult);
                _routineSuccesss();
            },

            (checkIDRedundancyFailure) => 
            {
                OnDebugPlayFabFailure(checkIDRedundancyFailure, "CheckIDRedundancyRequest");
                _routineFailure(checkIDRedundancyFailure.ErrorMessage);
            }
        );
    }

    private void CheckDisplayNameRequest(string _playfabID, Action<GetPlayerProfileResult> routineSuccess, Action<PlayFabError> routineFailure)
    {
        var request = new GetPlayerProfileRequest
        {
            PlayFabId = _playfabID,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetPlayerProfile(request, routineSuccess, routineFailure);
        OnDebugRequestSend("CheckEmailStatusRequest");
    }

    #region ID Redundancy Request

    private void CheckIDRedundancyRequest(string _playfabID, Action<GetPlayerProfileResult> routineSuccess, Action<PlayFabError> routineFailure)
    {
        var request = new GetPlayerProfileRequest
        {
            PlayFabId = _playfabID
        };
        PlayFabClientAPI.GetPlayerProfile(request, routineSuccess, routineFailure);
        OnDebugRequestSend("CheckIDRedundancyRequest");
    }

    #endregion


    #region Register Request
    private void SendRegisterRequest(PlayFabUserRegister _userRegister, Action<RegisterPlayFabUserResult> routineSuccess, Action<PlayFabError> routineFailure)
    {
        var request = new RegisterPlayFabUserRequest { Username = _userRegister.UserAccount.UserID, Password = _userRegister.UserAccount.UserPassword, Email = _userRegister.UserEmail, DisplayName = _userRegister.UserDisplayName };
        PlayFabClientAPI.RegisterPlayFabUser(request, 
            
            routineSuccess, routineFailure);
        OnDebugRequestSend("SendRegisterRequest");
    }

    private void GetRegisterEmailReqeust(string _playfabID, Action<GetAccountInfoResult> routineSuccess, Action<PlayFabError> routineFailure)
    {
        var request = new GetAccountInfoRequest { PlayFabId = _playfabID };
        PlayFabClientAPI.GetAccountInfo(request, routineSuccess, routineFailure);
        OnDebugRequestSend("GetRegisterEmailReqeust");
    }

    private void SendEmailVerificationRequest(string _userEmail, Action<AddOrUpdateContactEmailResult> routineSuccess, Action<PlayFabError> routineFailure)
    {
        var request = new AddOrUpdateContactEmailRequest { EmailAddress = _userEmail };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, routineSuccess, routineFailure);
        OnDebugRequestSend("SendEmailVerificationRequest");
    }

    #endregion

    #region Login Request
    private void SendLoginRequest(PlayFabUserAccount _userAccount, Action<LoginResult> routineSuccess, Action<PlayFabError> routineFailure)
    {
        var request = new LoginWithPlayFabRequest { Username = _userAccount.UserID, Password = _userAccount.UserPassword};
        PlayFabClientAPI.LoginWithPlayFab(request, routineSuccess, routineFailure);
        OnDebugRequestSend("SendLoginRequest");
    }

    private void CheckEmailStatusRequest(string _playfabID, Action<GetPlayerProfileResult> routineSuccess, Action<PlayFabError> routineFailure)
    {
        var request = new GetPlayerProfileRequest
        {
            PlayFabId = _playfabID,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true,
                ShowContactEmailAddresses = true
            }
        };
        PlayFabClientAPI.GetPlayerProfile(request, routineSuccess, routineFailure);
        OnDebugRequestSend("CheckEmailStatusRequest");
    }

    //private void OnCheckEmailStatusSuccess(GetPlayerProfileResult result, Action _verificationConfirmed, Action _verificationNotConfirmed)
    //{
    //    Debug.Log(OnDebugRequestSuccess("CheckEmailStatusRequest"));

    //    if (result.PlayerProfile.ContactEmailAddresses[0].VerificationStatus == EmailVerificationStatus.Confirmed)
    //    {
    //        _verificationConfirmed();
    //        return;
    //    }
    //    _verificationNotConfirmed();
    //}

    #endregion

}