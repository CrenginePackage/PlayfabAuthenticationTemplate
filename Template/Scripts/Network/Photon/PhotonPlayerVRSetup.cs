using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Crengine.Core;
//using Crengine.Util.KeyboardSystem;

public class PhotonPlayerVRSetup : PhotonPlayerSetup
{
    [Header("VR Components")]
    [SerializeField] private GameObject VRKeyboardCanvas;
    //[SerializeField] private VRKeyboard[] keyboards;

    //private KeyboardManager keyboardManager;

    protected override void PhotonSetup()
    {
        base.PhotonSetup();
        StartCoroutine(SetupRoutine());
    }

    private IEnumerator SetupRoutine()
    {
        yield return null;
        SetVRKeyboard();
    }

    private void SetVRKeyboard()
    {
        //keyboardManager = SingletonRegistry.instance.FindManagersOfType<KeyboardManager>();
        //keyboardManager.keyboardCanvas = VRKeyboardCanvas;
        //keyboardManager.targetKeyboard = keyboards[0];
        //keyboardManager.keyboards = keyboards;
        //keyboardManager.keyboardLocationOffset = new Vector3(.875f, -.3f, -.5f);
    }
}
