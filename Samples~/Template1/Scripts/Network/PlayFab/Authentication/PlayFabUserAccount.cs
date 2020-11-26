public readonly struct PlayFabUserAccount
{
    public string UserID { get; }
    public string UserPassword { get; }


    public PlayFabUserAccount(string _userID, string _userPassword)
    {
        this.UserID = _userID;
        this.UserPassword = _userPassword;
    }
}
