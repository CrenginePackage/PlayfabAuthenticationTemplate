using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crengine.Core;
using UnityEngine.EventSystems;
using TMPro;

namespace Crengine.Util.KeyboardSystem
{
    public interface IKeyboard
    {
        void OnShift();
        void OnAlt();
        void OnTap();
        void OnChangeCapital();
        void OnCtrl();
        void OnSpace();
        void OnEnter();
    }
    public class VRKeyboard : MonoBehaviour , IKeyboard
    {
        
        VRKey[] keys;
        [SerializeField]
        bool capital = false;
        KeyboardManager manager;

        void Start()
        {
            keys = GetComponentsInChildren<VRKey>();
            manager = SingletonRegistry.instance.FindManagersOfType<KeyboardManager>();
        }
        public void OnShift()
        {

        }
        public void OnAlt()
        {
            if (manager.currentLanguage == 0)
                manager.ChangeKeyboard(1);
            else
                manager.ChangeKeyboard(0);

            Debug.Log(manager.currentLanguage);

        }
        public void OnTap()
        {

        }
        public void OnChangeCapital()
        {
            for(int i = 0; i< keys.Length;i++)
            {
                capital = !capital;
                keys[i].SetKey(capital);
            }
        }
        public void OnCtrl()
        {
            SingletonRegistry.instance.FindManagersOfType<KeyboardManager>().ChangeKeyboard(2);
        }

        public void OnSpace()
        {

        }
        public void OnEnter()
        {

        }

        
    }
}
