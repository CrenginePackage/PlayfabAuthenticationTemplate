using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Crengine.Core;

namespace Crengine.Util.KeyboardSystem
{
    public class VRKeyboardCanvas : MonoBehaviour
    {
        KeyboardManager manager;
        private void Start()
        {
            manager = SingletonRegistry.instance.FindManagersOfType<KeyboardManager>();
        }
        
    }

}
