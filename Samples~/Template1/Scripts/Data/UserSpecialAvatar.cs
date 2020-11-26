using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class UserSpecialAvatar : IPlayFabData
{
    public int JsonVersion { get; set; }
    public SpecialAvatarNumber SpecialAvatarSelected { get; set; }
    public List<int> SpecialAvatarsAllowed { get; set; }

    public UserSpecialAvatar()
    {
        this.JsonVersion = 0;
        this.SpecialAvatarSelected = SpecialAvatarNumber.General;
        this.SpecialAvatarsAllowed = new List<int>();
    }

    public UserSpecialAvatar(int _jsonVersion, SpecialAvatarNumber _specialAvatarSelected, List<int> _specialAvatarsAllowed)
    {
        this.JsonVersion = _jsonVersion;
        this.SpecialAvatarSelected = _specialAvatarSelected;
        this.SpecialAvatarsAllowed = _specialAvatarsAllowed;
    }


    public Dictionary<string, string> GetDictionary()
    {
        return new Dictionary<string, string>()
        {
            {"UserSpecialAvatar", JsonConvert.SerializeObject(new UserSpecialAvatar(JsonVersion, SpecialAvatarSelected, SpecialAvatarsAllowed))}
        };
    }
}
