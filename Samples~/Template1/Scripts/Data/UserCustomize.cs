using System.Collections;
using System.Collections.Generic;

public class UserCustomize : IPlayFabData
{
    public int userGender { get; set; }
    public int avatarPreset { get; set; }
    public CustomizeColor upperColor { get; set; }

    public UserCustomize()
    {
        this.userGender = 0;
        this.avatarPreset = 0;
        this.upperColor = 0;
    }

    public UserCustomize(string customizeColors)
    {
        this.userGender = StringToInt(customizeColors, 0);
        this.avatarPreset = StringToInt(customizeColors, 1);
        this.upperColor = StringToColor(customizeColors, 2);
    }

    public Dictionary<string, string> GetDictionary()
    {
        return new Dictionary<string, string>()
        {
            {"UserCustomize", GetUserCustomize()}
        };
    }

    public string GetUserCustomize()
    {
        return IntToString(userGender) + IntToString(avatarPreset) + ColorToString(upperColor);
    }

    private string ColorToString(CustomizeColor _colorValue)
    {
        return ((int)_colorValue).ToString();
    }

    private string IntToString(int _IntValue)
    {
        return (_IntValue).ToString();
    }

    private CustomizeColor StringToColor(string colorString, int customizeNumber)
    {
        return (CustomizeColor) int.Parse(colorString[customizeNumber].ToString());
    }

    private int StringToInt(string colorString, int customizeNumber)
    {
        return int.Parse(colorString[customizeNumber].ToString());
    }

}
