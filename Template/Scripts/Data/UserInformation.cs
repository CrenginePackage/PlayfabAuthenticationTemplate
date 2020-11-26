using System.Collections;
using System.Collections.Generic;

public enum UserType
{
    Standard = 0,
    Business = 1,
    VIP = 2,
    Administrator = 3
}

public class UserInformation : IPlayFabData
{
    public string UserAddress { get; set; }
    public string UserName { get; set; }
    public UserType UserType { get; set; }


    public UserInformation()
    {
        this.UserAddress = "";
        this.UserName = "";
        this.UserType = UserType.Standard;
    }

    public UserInformation(string _userAddress, string _userName, UserType _userType)
    {
        this.UserAddress = _userAddress;
        this.UserName = _userName;
        this.UserType = _userType;
    }

    public UserInformation(string _userAddress, string _userName, int _userType)
    {
        this.UserAddress = _userAddress;
        this.UserName = _userName;
        this.UserType = (UserType)_userType;
    }

    public Dictionary<string, string> GetDictionary()
    {
        return new Dictionary<string, string>()
        {
            {"UserAddress", UserAddress },
            {"UserName", UserName },
            {"UserType", ((int) UserType).ToString()}
        };
    }
}
