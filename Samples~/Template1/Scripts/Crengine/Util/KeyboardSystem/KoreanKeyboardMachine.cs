using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crengine.Util.KeyboardSystem
{
    public class CalutateMap
    {
        public int[,] binary = new int[78, 78];
        public int[][] reverseBinary = new int[78][];

        public CalutateMap(int[,] _targetArray)
        {
            for (int i = 0; i < _targetArray.Length / 3; i++)
            {
                binary[_targetArray[i, 0], _targetArray[i, 1]] = _targetArray[i, 2];
                reverseBinary[_targetArray[i, 2]] = new int[] { _targetArray[i, 0], _targetArray[i, 1] };
            }
        }
    }

    public static class KoreanKeyboardMachine
    {
        static Dictionary<char, int> KRFirstMap = new Dictionary<char, int>
        {
            {'ㄱ',0},
            {'ㄲ',1},
            {'ㄴ',2},
            {'ㄷ',3},
            {'ㄸ',4},
            {'ㄹ',5},
            {'ㅁ',6},
            {'ㅂ',7},
            {'ㅃ',8},
            {'ㅅ',9},
            {'ㅆ',10},
            {'ㅇ',11},
            {'ㅈ',12},
            {'ㅉ',13},
            {'ㅊ',14},
            {'ㅋ',15},
            {'ㅌ',16},
            {'ㅍ',17},
            {'ㅎ',18}
        };


        static Dictionary<char, int> KRSecondMap = new Dictionary<char, int>
        {
            {'ㅏ',20},
            {'ㅐ',21},
            {'ㅑ',22},
            {'ㅒ',23},
            {'ㅓ',24},
            {'ㅔ',25},
            {'ㅕ',26},
            {'ㅖ',27},
            {'ㅗ',28},
            {'ㅘ',29},
            {'ㅙ',30},
            {'ㅚ',31},
            {'ㅛ',32},
            {'ㅜ',33},
            {'ㅝ',34},
            {'ㅞ',35},
            {'ㅟ',36},
            {'ㅠ',37},
            {'ㅡ',38},
            {'ㅢ',39},
            {'ㅣ',40}
        };


        static Dictionary<char, int> KRLastMap = new Dictionary<char, int>
        {
            {' ',50},
            {'ㄱ',51},
            {'ㄲ',52},
            {'ㄳ',53},
            {'ㄴ',54},
            {'ㄵ',55},
            {'ㄶ',56},
            {'ㄷ',57},
            {'ㄹ',58},
            {'ㄺ',59},
            {'ㄻ',60},
            {'ㄼ',61},
            {'ㄽ',62},
            {'ㄾ',63},
            {'ㄿ',64},
            {'ㅀ',65},
            {'ㅁ',66},
            {'ㅂ',67},
            {'ㅄ',68},
            {'ㅅ',69},
            {'ㅆ',70},
            {'ㅇ',71},
            {'ㅈ',72},
            {'ㅊ',73},
            {'ㅋ',74},
            {'ㅌ',75},
            {'ㅍ',76},
            {'ㅎ',77}
        };

        static char[] KRReversedMap = new char[]
        {
            'ㄱ',
            'ㄲ',
            'ㄴ',
            'ㄷ',
            'ㄸ',
            'ㄹ',
            'ㅁ',
            'ㅂ',
            'ㅃ',
            'ㅅ',
            'ㅆ',
            'ㅇ',
            'ㅈ',
            'ㅉ',
            'ㅊ',
            'ㅋ',
            'ㅌ',
            'ㅍ',
            'ㅎ',
            ' ',
            'ㅏ',
            'ㅐ',
            'ㅑ',
            'ㅒ',
            'ㅓ',
            'ㅔ',
            'ㅕ',
            'ㅖ',
            'ㅗ',
            'ㅘ',
            'ㅙ',
            'ㅚ',
            'ㅛ',
            'ㅜ',
            'ㅝ',
            'ㅞ',
            'ㅟ',
            'ㅠ',
            'ㅡ',
            'ㅢ',
            'ㅣ',
        };

        static Dictionary<int, int> KRMatchLastAndFirst = new Dictionary<int, int>
        {
            { 51,0},
            { 52,1},
            { 54,2},
            { 57,3},
            { 58,5},
            { 66,6},
            { 67,7},
            { 69,9},
            { 70,10},
            { 71,11},
            { 72,12},
            { 73,14},
            { 74,15},
            { 75,16},
            { 76,17},
            { 77,18}
        };

        // 초성,중성 모음 합성 테이블
        static int[,] MIXED_Second = new int[6, 3] {
        {28,20,29},	// ㅘ
        {28,21,30},	// ㅙ
        {34,24,34},	// ㅝ
        {34,25,35}, //ㅞ
        {34,40,36},	//ㅟ
        {38,40,39} //ㅢ
       };

        // 종성 합성 테이블
        static int[,] MIXED_LAST = new int[11, 3] {
           { 51,69,53},//ㄳ
           { 54,72,55},//ㄵ
           { 54,77,56},//ㄶ
           { 58,51,59},//ㄺ
           { 58,66,60},//ㄻ
           { 58,67,61},//ㄼ
           { 58,69,62},//ㄽ
           { 58,75,63},//ㄾ
           { 58,76,64},//ㄿ
           { 58,77,65},//ㅀ
           { 67,69,68},//ㅄ
       };


        private static CalutateMap secondCalculatedBinary = new CalutateMap(MIXED_Second);
        private static CalutateMap lastCalculatedBinary = new CalutateMap(MIXED_LAST);

        static List<int> resultList = new List<int>();

        public enum ResultState
        {
            Add,
            Delete,
            Correction
        }
        static ResultState AddIndexList(char _char)
        {
            if (resultList.Count == 0)
            {
                
                // resultList = new List<int>();
                if (KRFirstMap.ContainsKey(_char))
                {
                    resultList.Add(KRFirstMap[_char]);
                    return ResultState.Add;
                }
                if (KRSecondMap.ContainsKey(_char))
                {
                    resultList.Add(KRSecondMap[_char]);
                    return ResultState.Add;
                }

            }
            ResultState stepState = CheckCombinable(resultList[resultList.Count - 1], _char);
            if (stepState != ResultState.Add)
            {
                return stepState;
            }
            stepState = CheckAddable(resultList[resultList.Count - 1], _char);
            if (stepState != ResultState.Add)
            {
                return stepState;
            }

            return ResultState.Add;
        }

        static ResultState CheckCombinable(int _origin, char _target)
        {
            int combinedIndex;
            if (KRSecondMap.ContainsKey(_target))//모음 중성 합성 
            {
                combinedIndex = secondCalculatedBinary.binary[_origin, KRSecondMap[_target]];
                if (combinedIndex != 0)
                {
                    resultList[resultList.Count - 1] = combinedIndex;
                    return ResultState.Delete;
                }
            }

            if (KRLastMap.ContainsKey(_target))//자음 종성 합성
            {
                combinedIndex = lastCalculatedBinary.binary[_origin, KRLastMap[_target]];
                if (combinedIndex != 0)
                {

                    resultList[resultList.Count - 1] = combinedIndex;
                    return ResultState.Delete;
                }
            }

            return ResultState.Add;
        }

        static ResultState CheckAddable(int _origin, char _target)
        {
            if (_origin < 20)// 초성 뒤 중성
            {
                if (KRSecondMap.ContainsKey(_target))
                {
                    resultList.Add(KRSecondMap[_target]);
                    return ResultState.Delete;
                }
            }
            if (_origin >= 20 && _origin < 50) // 중성 뒤 종성 
            {
                if (KRLastMap.ContainsKey(_target))
                {
                    resultList.Add(KRLastMap[_target]);
                    return ResultState.Delete;
                }
            }
            else // 종성 뒤 모음 중성
            {
                if (KRSecondMap.ContainsKey(_target))
                {
                    if (lastCalculatedBinary.reverseBinary[resultList[resultList.Count - 1]] != null)//합성자
                    {
                        List<int> dividedIndex = new List<int>() { lastCalculatedBinary.reverseBinary[resultList[resultList.Count - 1]][0], lastCalculatedBinary.reverseBinary[resultList[resultList.Count - 1]][1] };
                        resultList.InsertRange(resultList.Count - 1, dividedIndex);
                        resultList.Remove(resultList[resultList.Count - 1]);
                    }
                    resultList[resultList.Count - 1] = KRMatchLastAndFirst[resultList[resultList.Count - 1]];
                    resultList.Add(KRSecondMap[_target]);

                    return ResultState.Correction;
                }
            }
            return ResultState.Add;
        }

        public static ResultState ComputeKorean(char _key, ref char _result, ref char _correctionResult)
        {

            char keyChar = _key;

            switch (AddIndexList(keyChar))
            {

                case ResultState.Delete:
                    _result = CalculateKorean(resultList);
                    return ResultState.Delete;
                case ResultState.Correction:
                    List<int> lastIndexList = resultList.GetRange(resultList.Count - 2, 2);
                    resultList.RemoveRange(resultList.Count - 2, 2);
                    _correctionResult = CalculateKorean(resultList);
                    resultList = new List<int>();
                    resultList.AddRange(lastIndexList);
                    _result = CalculateKorean(resultList);
                    return ResultState.Correction;
                case ResultState.Add:
                    resultList = new List<int>();
                    AddIndexList(keyChar);
                    _result = CalculateKorean(resultList);
                    return ResultState.Add;

            }
            return ResultState.Add;

        }

        static char CalculateKorean(List<int> _indexList)
        {

            char result = '가';

         if(_indexList.Count == 0)
            {
                return ' ';
            }

            if (_indexList.Count == 1)
            {
                return (KRReversedMap[_indexList[0]]);
            }
            if (_indexList.Count >= 2)
            {
                result += (char)(((_indexList[0] * 21) + (_indexList[1] - 20)) * 28);
            }
            if (_indexList.Count >= 3)
            {
                result += (char)(_indexList[2] - 50);

            }
            return result;
        }

        public static ResultState BackSpace(ref char _result)
        {
            if (resultList.Count <= 1)
            {
                return ResultState.Delete;
            }
            resultList.RemoveAt(resultList.Count - 1);
            Debug.Log(resultList.Count);
            _result = CalculateKorean(resultList);
            return ResultState.Correction;

        }

        public static bool Reset(ref char _result)
        {
            bool isEmpty = false;
            if (resultList.Count == 0)
                isEmpty = true;
            _result = CalculateKorean(resultList);
            resultList = new List<int>();
            return isEmpty;
        }
    }
}
