using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crengine.Core;
using UnityEngine.XR;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Crengine.Util.KeyboardSystem
{

    public class KeyEvent : EventCore
    {
       
        KeyCode keyFunction;
     

        public KeyEvent(KeyCode _keyFunction)
        {
            keyFunction = _keyFunction;
        }
    }
    
    public class KeyboardManager : Manager
    {
        public GameObject keyboardCanvas;
        public VRKeyboard targetKeyboard;
        public VRKeyboard[] keyboards;
        public TMP_InputField targetInputField;
        public int currentLanguage;
        public Vector3 keyboardLocationOffset;

        private void Start()
        {
            if (!XRSettings.enabled)
            {
                this.enabled = false;
            }
        }

        public void ChangeKeyboard(int _index)
        {

            targetKeyboard.gameObject.SetActive(false);
            keyboards[_index].gameObject.SetActive(true);
            targetKeyboard = keyboards[_index];
            currentLanguage = _index;
            //Debug.Log(_index);
            
        }

        public void OpenKeyboard(Transform _keyboardLocation)
        {
            targetInputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
            targetInputField.onFocusSelectAll = false;
            targetKeyboard.gameObject.SetActive(true);

            SetKeyboardTransform(_keyboardLocation, keyboardLocationOffset);
        }

        private void SetKeyboardTransform(Transform _keyboardLocation, Vector3 _positionOffset)
        {
            keyboardCanvas.transform.position = _keyboardLocation.position + _keyboardLocation.forward  * _positionOffset.z + _keyboardLocation.right * _positionOffset.x + _keyboardLocation.up * _positionOffset.y;
            keyboardCanvas.transform.rotation = _keyboardLocation.rotation;
        }

        public void CloseKeyboard()
        {
            if(targetInputField != null)
                targetInputField.onFocusSelectAll = true;
            targetInputField = null;
            targetKeyboard.gameObject.SetActive(false);
        }

        public void OnGUI()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                TMP_InputField currentSelectedInputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

                if (currentSelectedInputField != null && !currentSelectedInputField.Equals(targetInputField) && currentSelectedInputField.isFocused != false)
                {
                    OpenKeyboard(currentSelectedInputField.transform);
                    Input.imeCompositionMode = IMECompositionMode.On;
                    isRemoved = false;
                }
            }
            else
            {
                if (!isRemoved)
                {
                    CloseKeyboard();
                    isRemoved = true;
                }
            }
        }

        bool isRemoved;
    }
}
