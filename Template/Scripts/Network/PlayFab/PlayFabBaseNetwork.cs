using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;

public class PlayFabBaseNetwork : MonoBehaviour
{
    #region Debug

    protected void OnDebugRequestSuccess(string request)
    {
        OnDebugComment(request + " Success !");
    }

    protected void OnDebugRequestSend(string request)
    {
        OnDebugComment("Sending " + request + "...");
    }

    protected void OnDebugComment(string comment)
    {
        Debug.Log("PlayFab: " + comment);
    }

    protected void OnDebugPlayFabFailure(PlayFabError error, string networkAction)
    {
        Debug.LogError("Playfab Error (" + networkAction + ") : ErrorCode <" + error.Error + ">");
        Debug.LogError("Playfab Error - " + error.GenerateErrorReport());
    }

    private string ConvertErrorDetails(Dictionary<string, List<string>> errorDetails)
    {
        string error = "";
        foreach(KeyValuePair<string, List<string>> errorDetail in errorDetails)
        {
            error += "[" + errorDetail.Key + "] = { ";

            for (int i = 0; i < errorDetail.Value.Count; i++)
            {
                error += errorDetail.Value[i] + " | "; 
            }

            error += " } &&";
        }

        return error;
    }



    #endregion
}
