[System.Serializable]
public class PlayFabPlayerData
{
    public UserCustomize UserCustomize { get; set; }
    public UserInformation UserInformation { get; set; }
    public UserSpecialAvatar UserSpecialAvatar { get; set; }
    
    public string UserDisplayName { get; set; }

    public PlayFabPlayerData()
    {
        this.UserCustomize = new UserCustomize();
        this.UserInformation = new UserInformation();
        this.UserSpecialAvatar = new UserSpecialAvatar();
        this.UserDisplayName = "Name#123456";
    }

    public PlayFabPlayerData(UserCustomize _userCustomize, UserInformation _userInformation, UserSpecialAvatar _userSpecialAvatar, string _userDisplayName)
    {
        this.UserCustomize = _userCustomize;
        this.UserInformation = _userInformation;
        this.UserSpecialAvatar = _userSpecialAvatar;
        this.UserDisplayName = _userDisplayName;
    }
}
