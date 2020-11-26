public readonly struct PlayFabUserRegister
{
    public PlayFabUserAccount UserAccount { get; }
    public string UserEmail { get; }
    public string UserDisplayName { get; }
    public UserInformation UserInformation { get; }

    public PlayFabUserRegister(string _userID, string _userPassword, string _userEmail, string _userDisplayName, UserInformation _userInformation)
    {
        this.UserAccount = new PlayFabUserAccount(_userID, _userPassword);
        this.UserEmail = _userEmail;
        this.UserDisplayName = _userDisplayName;
        this.UserInformation = _userInformation;
    } 

    public PlayFabUserRegister(PlayFabUserAccount _userAccount, string _userEmail, string _userDisplayName)
    {
        this.UserAccount = _userAccount;
        this.UserEmail = _userEmail;
        this.UserDisplayName = _userDisplayName;
        this.UserInformation = new UserInformation();
    }
    
}
