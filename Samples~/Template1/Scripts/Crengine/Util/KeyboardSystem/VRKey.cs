using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crengine.Core;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Crengine.Util.KeyboardSystem
{

    public class VRKey : MonoBehaviour
    {
       
        protected enum KEYTYPE
        {
            Non,
            Shift,
            Tap,
            Crtl,
            Alt
        }
        [SerializeField]
        protected KeyCode key;
        char keyChar;
        protected string keyString;
        [SerializeField]
        bool IsCapital;
        [SerializeField]
        protected bool IsTextButton;
        [SerializeField]
        protected TMP_Text Text;
        Event keyEvent;
        char preChar=(char)0;
        protected KeyboardManager keyboardManager;
        IKeyboard keyboard;

        protected delegate void Click();
        protected Click ClickFunction;

  
        private void Awake()
        {
            keyboardManager = SingletonRegistry.instance.FindManagersOfType<KeyboardManager>();
            Debug.Log(keyboardManager);
            keyboard = keyboardManager.targetKeyboard;
            InitKey();
        }

        protected virtual void InitKey()
        {
            SetKey(false);
            ClickFunction += RaiseCommonEvent;
        }

        public void OnClick()
        {
            //Debug.Log(EventSystem.current);
            EventSystem.current.SetSelectedGameObject(keyboardManager.targetInputField.gameObject);
            ClickFunction();
        }

        public void SetKey(bool _capitalOn)
        {
            IsCapital = _capitalOn;
            keyChar = SetCapitalChar();
            keyString = keyChar.ToString();
            if (IsTextButton)
            {
                Text.text = keyString;
            }

        }

        char SetCapitalChar()
        {
            char result = (char)key;
            if (key < KeyCode.A || key > KeyCode.Z)
            {
                return result;
            }

            if (IsCapital)
            {
                result += (char)32;
            }
            return result;
        }
        void RaiseCommonEvent()
        {
            RaiseEvent(key, (char)key);
        }

        protected void RaiseEvent(KeyCode _key, char _keyChar)
        {
            keyEvent = new Event();
            keyEvent.keyCode = _key;
            keyEvent.type = EventType.KeyDown;
            keyEvent.character = '\0';
            

            if ((int)_key >= 33 && (int)_key <= 126)
            {
                keyEvent.character = _keyChar;
                keyEvent.modifiers = EventModifiers.None;
            }
            else if (key == KeyCode.CapsLock)
            {
                keyEvent.modifiers = EventModifiers.CapsLock;
            }
            else if (key == KeyCode.LeftShift || key == KeyCode.RightShift)
            {
                keyEvent.modifiers = EventModifiers.Shift;
                keyboard.OnShift();
            }
            else if (key == KeyCode.LeftControl || key == KeyCode.RightControl)
            {
                keyEvent.modifiers = EventModifiers.Control;
                keyboard.OnCtrl();
            }
            else if (key == KeyCode.LeftAlt || key == KeyCode.RightAlt)
            {
                keyEvent.modifiers = EventModifiers.Alt;
                keyboard.OnAlt();
            }
            else if (key == KeyCode.Space)
            {
                keyEvent.character = (char)KeyCode.Space;
                keyboard.OnSpace();
            }
            else if (key == KeyCode.Return)
            {
                keyEvent.character = (char)KeyCode.Return;
                keyboard.OnEnter();
            }
            else if(key == KeyCode.Backspace)
            {

            }
            else
            {
                keyEvent.modifiers = EventModifiers.FunctionKey;
            }
            if (keyboardManager.targetInputField != null)
                keyboardManager.targetInputField.ProcessEvent(keyEvent);
        }
    }
}
