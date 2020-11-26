using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crengine.Core;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Crengine.Util.KeyboardSystem
{

    public class VRKoreanKey : VRKey
    {
       

        static Dictionary<KeyCode, char> koreanKeyMap = new Dictionary<KeyCode, char> {
           { KeyCode.Q, 'ㅂ' },
           { KeyCode.W, 'ㅈ' },
           { KeyCode.R, 'ㄱ' },
           { KeyCode.E, 'ㄷ' },
           { KeyCode.T, 'ㅅ' },
           { KeyCode.Q+32, 'ㅃ' },
           { KeyCode.W+32, 'ㅉ' },
           { KeyCode.R+32, 'ㄲ' },
           { KeyCode.E+32, 'ㄸ' },
           { KeyCode.T+32, 'ㅆ' },
           { KeyCode.Y, 'ㅛ' },
           { KeyCode.U, 'ㅕ' },
           { KeyCode.I, 'ㅑ' },
           { KeyCode.O, 'ㅐ' },
           { KeyCode.P, 'ㅔ' },
           { KeyCode.A, 'ㅁ' },
           { KeyCode.S, 'ㄴ' },
           { KeyCode.D, 'ㅇ' },
           { KeyCode.F, 'ㄹ' },
           { KeyCode.G, 'ㅎ' },
           { KeyCode.H, 'ㅗ' },
           { KeyCode.J, 'ㅓ' },
           { KeyCode.K, 'ㅏ' },
           { KeyCode.L, 'ㅣ' },
           { KeyCode.Z, 'ㅋ' },
           { KeyCode.X, 'ㅌ' },
           { KeyCode.C, 'ㅊ' },
           { KeyCode.V, 'ㅍ' },
           { KeyCode.B, 'ㅠ' },
           { KeyCode.N, 'ㅜ' },
           { KeyCode.M, 'ㅡ' }
       };


        protected override void InitKey()
        {
           SetKoreanKey(false);
           Debug.Log("settingKorean");
           ClickFunction += RaiseKoreanKeyEvent;
        }

        public void RaiseKoreanKeyEvent()
        {
            char result = (char)key;
            char collectionResult = ' ';
            if (key >= KeyCode.A && key <= KeyCode.Z)
            {
                switch (KoreanKeyboardMachine.ComputeKorean(koreanKeyMap[key], ref result, ref collectionResult))
                {
                    case KoreanKeyboardMachine.ResultState.Add:
                        break;
                    case KoreanKeyboardMachine.ResultState.Delete:
                        RaiseEvent(KeyCode.Backspace, '\b');
                        break;
                    case KoreanKeyboardMachine.ResultState.Correction:
                        char[] currentTextChar = keyboardManager.targetInputField.text.ToCharArray();
                        currentTextChar[currentTextChar.Length - 1] = collectionResult;
                        keyboardManager.targetInputField.text = currentTextChar.ArrayToString();
                        break;
                }
            }
            switch (key) // functionKey
            {
                case KeyCode.Backspace:
                   RaiseEvent(KeyCode.Backspace, '\b');
                   switch(KoreanKeyboardMachine.BackSpace(ref result)) 
                   {
                        case KoreanKeyboardMachine.ResultState.Correction:
                            RaiseEvent(KeyCode.A, result);
                            Debug.Log("Delete : Correction");
                            break;
                        case KoreanKeyboardMachine.ResultState.Delete:
                            KoreanKeyboardMachine.Reset(ref result);
                            Debug.Log("Delete : Delete");
                            break;
                        default:
                            Debug.Log("Delete : Add");
                            break;
                   }
                //    KoreanKeyboardMachine.Reset(ref result);
                   return;
                case KeyCode.Space:
                    bool emptyCheck = KoreanKeyboardMachine.Reset(ref result);
                    Debug.Log(emptyCheck);
                   if (emptyCheck)
                   {
                        
                        RaiseEvent(KeyCode.Space, '\0');
                    }
                    return;
            }
            RaiseEvent(key, result);
        }
        public void SetKoreanKey(bool _captialOn)
        {
            SetKey(_captialOn);
            if(koreanKeyMap.ContainsKey(key))
             keyString = koreanKeyMap[key].ToString();
            if (IsTextButton)
                Text.text = keyString;
        }
        private void OnDisable()
        {
            char result = ' ';
            KoreanKeyboardMachine.Reset(ref result);
        }
    }
}
