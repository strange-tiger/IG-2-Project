using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class KeyboardManager : GlobalInstance<KeyboardManager>
{
    public enum EKeyboardLayout
    {
        QWERTY,
        QWERTY_SHIFTED,
        KOREAN,
        KOREAN_SHIFTED,
        NUMPAD,
        MAX
    }

    private static TMP_InputField _inputField;
    private static EKeyboardLayout _currentLayout;

    private static TMP_InputField _typedText;
    private static Transform _keyboard;
    private static GameObject[] _layouts =
        new GameObject[(int)EKeyboardLayout.MAX];

    private void Start()
    {
        _keyboard = transform;

        _typedText = transform.GetChild(0).GetComponent<TMP_InputField>();
        _typedText.gameObject.SetActive(false);

        int j;
        for (int i = 1; i < transform.childCount; ++i)
        {
            j = i - 1;
            _layouts[j] = transform.GetChild(i).gameObject;

            _layouts[j].SetActive(false);
        }
    }

    /// <summary>
    /// 상위 UI의 인풋필드 OnSelect 이벤트를 구독, 인풋필드가 선택될 때 키보드를 연다.
    /// _inputField 변수에 선택된 인풋필드를 받는다.
    /// 열린 키보드의 위치를 MOVE_KEYBOARD로 지정한다.
    /// 기본 키보드 레이아웃은 QWERTY이다.
    /// </summary>
    private static readonly Vector3 MOVE_KEYBOARD = new Vector3(0f, -60f, -10f);
    public static void OpenKeyboard()
    {
        _inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
        _typedText.gameObject.SetActive(true);

        _keyboard.localPosition = MOVE_KEYBOARD;

        ChangeLayout(EKeyboardLayout.QWERTY);
    }

    /// <summary>
    /// 상위 UI의 인풋필드 OnSelect 이벤트를 구독, 인풋필드가 선택될 때 키보드 레이아웃을 활성화 한다.
    /// _inputField 변수에 선택된 인풋필드를 할당한다.
    /// 열린 키보드의 위치를 MOVE_KEYBOARD로 지정한다.
    /// type 매개변수로 활성화 할 키보드의 레이아웃을 정한다.
    /// </summary>
    /// <param name="type"></param>
    public static void OpenKeyboard(EKeyboardLayout type)
    {
        _inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

        _keyboard.localPosition = MOVE_KEYBOARD;

        _typedText.gameObject.SetActive(true);

        ChangeLayout(type);
    }

    /// <summary>
    /// 키보드 레이아웃을 type 매개변수로 받은 키보드 레이아웃으로 바꿔 활성화한다.
    /// 먼저 레이아웃을 모두 닫고, type 매개변수가 가리키는 레이아웃만 활성화한다.
    /// </summary>
    /// <param name="type"></param>
    private static void ChangeLayout(EKeyboardLayout type)
    {
        CloseLayout();

        _currentLayout = type;
        _layouts[(int)_currentLayout].SetActive(true);
    }

    /// <summary>
    /// 키보드 레이아웃을 모두 비활성화한다.
    /// 키보드 입력을 하며 저장된 데이터를 모두 비운다.
    /// 타입된 문자열을 저장하고 보여주는 텍스트 _typedText를 비활성화한다.
    /// </summary>
    public static void CloseKeyboard()
    {
        CloseLayout();
        _inputField = null;
        PressClear();
        _typedText.gameObject.SetActive(false);
    }

    /// <summary>
    /// _layouts에 저장된 이 키보드의 모든 키보드 레이아웃을 비활성화한다.
    /// </summary>
    private static void CloseLayout()
    {
        foreach (GameObject layout in _layouts)
        {
            layout.SetActive(false);
        }
    }

    /// <summary>
    /// 키보드의 자판 키 버튼이 클릭될 때마다, _typedText.text에 선택된 버튼의 이름을 저장한다.
    /// 따라서, 자판 키 버튼의 이름은 모두 입력하고 싶은 문자와 동일해야 한다.
    /// CutKoreanType로 더 이상 한국어 입력이 아님을 알린다.
    /// EventSystem으로부터 선택된 버튼의 정보를 지우고 끝낸다.
    /// </summary>
    public static void PressKey()
    {
        CutKoreanType();

        _typedText.text += EventSystem.current.currentSelectedGameObject.name;

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 키보드의 자판 키 버튼이 클릭될 때마다, s_koreanSentence 선택된 버튼의 이름을 저장한다.
    /// 따라서, 자판 키 버튼의 이름은 모두 입력하고 싶은 문자와 동일해야 한다.
    /// 지금부터 한국어 입력이 실행됨을 알린다. s_isKorean을 true로 바꾼다.
    /// _typedText.text에 저장된 이전 입력을 s_prevSentence에 저장한다.
    /// CreateKoreanText(s_koreanSentence)로 생성한 한글 문자열을 s_prevSentence과 함께 _typedText.text에 저장, 유저에게 보인다.
    /// EventSystem으로부터 선택된 버튼의 정보를 지우고 끝낸다.
    /// </summary>
    private static bool s_isKorean = false;
    private static string s_prevSentence = string.Empty;
    private static string s_koreanSentence = string.Empty;
    public static void PressKoreanKey()
    {
        if (!s_isKorean)
        {
            s_isKorean = !s_isKorean;

            s_prevSentence = _typedText.text;
        }

        s_koreanSentence += EventSystem.current.currentSelectedGameObject.name;

        _typedText.text = s_prevSentence + CreateKoreanText(s_koreanSentence);

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 지금부터 한국어 입력이 완료됨을 알린다.
    /// s_isKorean을 false로 바꾼다.
    /// s_prevSentence과 s_koreanSentence을 비운다.
    /// </summary>
    private static void CutKoreanType()
    {
        if (s_isKorean)
        {
            s_isKorean = !s_isKorean;

            s_prevSentence = string.Empty;
            s_koreanSentence = string.Empty;
        }
    }

    /// <summary>
    /// _typedText.text에 공백을 추가한다.
    /// CutKoreanType로 더 이상 한국어 입력이 아님을 알린다.
    /// EventSystem으로부터 선택된 버튼의 정보를 지우고 끝낸다.
    /// </summary>
    public static void PressSpace()
    {
        CutKoreanType();

        _typedText.text += " ";

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// _typedText.text에 저장된 문자열의 마지막 한 자리를 줄여 한 글자 지운다.
    /// CutKoreanType로 더 이상 한국어 입력이 아님을 알린다.
    /// EventSystem으로부터 선택된 버튼의 정보를 지우고 끝낸다.
    /// </summary>
    public static void PressBackspace()
    {
        CutKoreanType();

        if (_typedText.text.Length == 0) return;
        _typedText.text = _typedText.text.Substring(0, _typedText.text.Length - 1);

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// _typedText.text에 저장된 문자열을 모두 지운다.
    /// CutKoreanType로 더 이상 한국어 입력이 아님을 알린다.
    /// </summary>
    public static void PressClear()
    {
        CutKoreanType();

        if (_typedText.text.Length == 0) return;
        _typedText.text = string.Empty;
    }

    /// <summary>
    /// 키보드의 Shift 혹은 Caps Lock 역할을 한다.
    /// 레이아웃의 EKeyboardLayout 값을 1씩 줄이고 늘인다.
    /// _currentLayout이 EKeyboardLayout.QWERTY, KOREAN이면 각각 1씩 늘려 EKeyboardLayout.QWERTY_SHIFTED, KOREAN_SHIFTED로 바꾼다.
    /// _currentLayout이 EKeyboardLayout.QWERTY_SHIFTED, KOREAN_SHIFTED이면 각각 1씩 늘려 EKeyboardLayout.QWERTY, KOREAN로 바꾼다.
    /// </summary>
    public static void PressShift()
    {
        if (_currentLayout == EKeyboardLayout.QWERTY
            || _currentLayout == EKeyboardLayout.KOREAN)
        {
            ChangeLayout(_currentLayout + 1);
        }
        else if (_currentLayout == EKeyboardLayout.QWERTY_SHIFTED
            || _currentLayout == EKeyboardLayout.KOREAN_SHIFTED)
        {
            ChangeLayout(_currentLayout - 1);
        }
    }

    /// <summary>
    /// 지금까지 입력한 문자열을 UI의 인풋필드에 반영한다.
    /// _typedText.text에 저장된 문자열을 현재 선택된 인풋필드 _inputField.text에 할당하고
    /// 키보드를 닫는다.
    /// </summary>
    public static void Submit()
    {
        _inputField.text = _typedText.text;
        CloseKeyboard();
    }

    /// <summary>
    /// 키보드의 한영키 역할을 한다.
    /// _currentLayout이 EKeyboardLayout.QWERTY, QWERTY_SHIFTED이면 KOREAN으로 바꾼다.
    /// _currentLayout이 EKeyboardLayout.KOREAN, KOREAN_SHIFTED이면 QWERTY로 바꾼다.
    /// CutKoreanType로 더 이상 한국어 입력이 아님을 알린다.
    /// </summary>
    public static void ChangeLanguage()
    {
        CutKoreanType();

        if (_currentLayout == EKeyboardLayout.QWERTY
            || _currentLayout == EKeyboardLayout.QWERTY_SHIFTED)
        {
            ChangeLayout(EKeyboardLayout.KOREAN);
        }
        else if (_currentLayout == EKeyboardLayout.KOREAN
            || _currentLayout == EKeyboardLayout.KOREAN_SHIFTED)
        {
            ChangeLayout(EKeyboardLayout.QWERTY);
        }
    }

    #region 한글_입력

    private const string INITIAL_CONSONANT = "ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ";
    private const string NEUTRAL_VOWEL = "ㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅣ";
    private const string FINAL_CONSONANT = "ㄱㄲㄳㄴㄵㄶㄷㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅄㅅㅆㅇㅈㅊㅋㅌㅍㅎ";

    private static string CreateKoreanText(string input)
    {
        string convertString = string.Empty;
        string output = string.Empty;

        int index = 0;
        int length = input.Length;

        while (index < length)
        {
            switch (length - index)
            {
                case 0:
                    break;
                case 1:
                    {
                        int consonantIndex = FINAL_CONSONANT.IndexOf(input[index]);

                        int vowelIndex = NEUTRAL_VOWEL.IndexOf(input[index]);

                        if (consonantIndex != -1)
                        {
                            ++index;
                            convertString += FINAL_CONSONANT[consonantIndex];
                        }
                        else if (vowelIndex != -1)
                        {
                            ++index;
                            convertString += NEUTRAL_VOWEL[vowelIndex];
                        }

                        break;
                    }
                case 2:
                    {
                        int firstConsonantIndex = FINAL_CONSONANT.IndexOf(input[index]);
                        int secondConsonantIndex = FINAL_CONSONANT.IndexOf(input[index + 1]);

                        int firstVowelIndex = NEUTRAL_VOWEL.IndexOf(input[index]);
                        int secondVowelIndex = NEUTRAL_VOWEL.IndexOf(input[index + 1]);

                        if (firstConsonantIndex != -1 && secondConsonantIndex != -1)
                        {
                            int i = CombineTwoConsonant(firstConsonantIndex, secondConsonantIndex);
                            if (i == firstConsonantIndex)
                            {
                                ++index;
                            }
                            else
                            {
                                firstConsonantIndex = i;
                                index += 2;
                            }
                            convertString += FINAL_CONSONANT[firstConsonantIndex];
                        }
                        else if (firstVowelIndex != -1 && secondVowelIndex != -1)
                        {
                            int i = CombineTwoVowel(firstVowelIndex, secondVowelIndex);

                            if (i == firstVowelIndex)
                            {
                                ++index;
                            }
                            else
                            {
                                firstVowelIndex = i;
                                index += 2;
                            }
                            convertString += NEUTRAL_VOWEL[firstVowelIndex];
                        }
                        else
                        {
                            if (firstConsonantIndex != -1)
                            {
                                ++index;
                                convertString += FINAL_CONSONANT[firstConsonantIndex];
                            }
                            else if (firstVowelIndex != -1)
                            {
                                ++index;
                                convertString += NEUTRAL_VOWEL[firstVowelIndex];
                            }
                        }
                        break;
                    }
                case 3:
                default:
                    {
                        int firstConsonantIndex = FINAL_CONSONANT.IndexOf(input[index]);
                        int secondConsonantIndex = FINAL_CONSONANT.IndexOf(input[index + 1]);
                        int thirdConsonantIndex = FINAL_CONSONANT.IndexOf(input[index + 2]);

                        int firstVowelIndex = NEUTRAL_VOWEL.IndexOf(input[index]);
                        int secondVowelIndex = NEUTRAL_VOWEL.IndexOf(input[index + 1]);

                        if (firstConsonantIndex != -1 && secondConsonantIndex != -1 && thirdConsonantIndex != -1)
                        {
                            int i = CombineTwoConsonant(firstConsonantIndex, secondConsonantIndex);
                            if (i == firstConsonantIndex)
                            {
                                ++index;
                            }
                            else
                            {
                                firstConsonantIndex = i;
                                index += 2;
                            }
                            convertString += FINAL_CONSONANT[firstConsonantIndex];
                        }
                        else if (firstVowelIndex != -1 && secondVowelIndex != -1)
                        {
                            int i = CombineTwoVowel(firstVowelIndex, secondVowelIndex);

                            if (i == firstVowelIndex)
                            {
                                ++index;
                            }
                            else
                            {
                                firstVowelIndex = i;
                                index += 2;
                            }
                            convertString += NEUTRAL_VOWEL[firstVowelIndex];
                        }
                        else
                        {
                            if (firstConsonantIndex != -1)
                            {
                                ++index;
                                convertString += FINAL_CONSONANT[firstConsonantIndex];
                            }
                            else if (firstVowelIndex != -1)
                            {
                                ++index;
                                convertString += NEUTRAL_VOWEL[firstVowelIndex];
                            }
                        }
                        break;
                    }
            }
        }

        index = 0;
        length = convertString.Length;
        while (index < length)
        {
            switch (length - index)
            {
                case 0:
                    break;
                case 1:
                    {
                        int i = convertString[index];
                        char letter = Convert.ToChar(i);
                        output += letter.ToString();

                        ++index;

                        break;
                    }
                case 2:
                    {
                        int initialIndex = INITIAL_CONSONANT.IndexOf(convertString[index]);
                        int middleIndex = NEUTRAL_VOWEL.IndexOf(convertString[index + 1]);
                        int finalIndex = 0;

                        if (initialIndex != -1 && middleIndex != -1)
                        {
                            int i = CalculateLetterCode(initialIndex, middleIndex, finalIndex);
                            char letter = Convert.ToChar(i);
                            output += letter.ToString();

                            index += 2;
                        }
                        else
                        {
                            int i = convertString[index];
                            char letter = Convert.ToChar(i);
                            output += letter.ToString();

                            ++index;
                        }

                        break;
                    }
                case 3:
                default:
                    {
                        int initialIndex = INITIAL_CONSONANT.IndexOf(convertString[index]);
                        int middleIndex = NEUTRAL_VOWEL.IndexOf(convertString[index + 1]);
                        int finalConsonantIndex = FINAL_CONSONANT.IndexOf(convertString[index + 2]);
                        int finalVowelIndex = NEUTRAL_VOWEL.IndexOf(convertString[index + 2]);

                        if (initialIndex != -1 && middleIndex != -1 && finalVowelIndex != -1)
                        {
                            middleIndex = CombineTwoVowel(middleIndex, finalVowelIndex);

                            finalConsonantIndex = 0;

                            int i = CalculateLetterCode(initialIndex, middleIndex, finalConsonantIndex);
                            char letter = Convert.ToChar(i);
                            output += letter.ToString();

                            index += 3;
                        }
                        else if (initialIndex != -1 && middleIndex != -1 && finalConsonantIndex != -1)
                        {
                            ++finalConsonantIndex;

                            int i = CalculateLetterCode(initialIndex, middleIndex, finalConsonantIndex);
                            char letter = Convert.ToChar(i);
                            output += letter.ToString();

                            index += 3;
                        }
                        else
                        {
                            int i = convertString[index];
                            char letter = Convert.ToChar(i);
                            output += letter.ToString();

                            ++index;
                        }
                        break;
                    }
            }
        }

        return output;
    }

    private static int CalculateLetterCode(int first, int second, int third)
    {
        return 0xAC00 + (first * 588) + (second * 28) + third;
    }

    #region 이중자모음

    private static readonly Dictionary<int, int>[] DOUBLE_CONSONANT_INDEX = new Dictionary<int, int>[4]
    {
        new Dictionary<int, int>()
        {
            { FINAL_CONSONANT.IndexOf('ㅅ'), FINAL_CONSONANT.IndexOf('ㄳ') }
        },
        new Dictionary<int, int>()
        {
            { FINAL_CONSONANT.IndexOf('ㅈ'), FINAL_CONSONANT.IndexOf('ㄵ') },
            { FINAL_CONSONANT.IndexOf('ㅎ'), FINAL_CONSONANT.IndexOf('ㄶ') }
        },
        new Dictionary<int, int>()
        {
            { FINAL_CONSONANT.IndexOf('ㄱ'), FINAL_CONSONANT.IndexOf('ㄺ') },
            { FINAL_CONSONANT.IndexOf('ㅁ'), FINAL_CONSONANT.IndexOf('ㄻ') },
            { FINAL_CONSONANT.IndexOf('ㅂ'), FINAL_CONSONANT.IndexOf('ㄼ') },
            { FINAL_CONSONANT.IndexOf('ㅅ'), FINAL_CONSONANT.IndexOf('ㄽ') },
            { FINAL_CONSONANT.IndexOf('ㅌ'), FINAL_CONSONANT.IndexOf('ㄾ') },
            { FINAL_CONSONANT.IndexOf('ㅎ'), FINAL_CONSONANT.IndexOf('ㅀ') }
        },
        new Dictionary<int, int>()
        {
            { FINAL_CONSONANT.IndexOf('ㅅ'), FINAL_CONSONANT.IndexOf('ㅄ') }
        }
    };

    private enum FirstConsonant
    {
        ㄱ = 0,
        ㄴ = 3,
        ㄹ = 7,
        ㅂ = 16
    }

    private static int CombineTwoConsonant(int first, int second)
    {
        int outputValue = first;

        switch ((FirstConsonant)first)
        {
            case FirstConsonant.ㄱ:
                break;

            case FirstConsonant.ㄴ:
                first = 1;
                break;

            case FirstConsonant.ㄹ:
                first = 2;
                break;

            case FirstConsonant.ㅂ:
                first = 3;
                break;

            default:
                return outputValue;
        }

        if (DOUBLE_CONSONANT_INDEX[first].ContainsKey(second))
            outputValue = DOUBLE_CONSONANT_INDEX[first][second];

        return outputValue;
    }

    private static readonly Dictionary<int, int>[] DOUBLE_VOWEL_INDEX = new Dictionary<int, int>[3]
    {
        new Dictionary<int, int>()
        {
            { NEUTRAL_VOWEL.IndexOf('ㅏ'), NEUTRAL_VOWEL.IndexOf('ㅘ') },
            { NEUTRAL_VOWEL.IndexOf('ㅐ'), NEUTRAL_VOWEL.IndexOf('ㅙ') },
            { NEUTRAL_VOWEL.IndexOf('ㅣ'), NEUTRAL_VOWEL.IndexOf('ㅚ') }
        },
        new Dictionary<int, int>()
        {
            { NEUTRAL_VOWEL.IndexOf('ㅓ'), NEUTRAL_VOWEL.IndexOf('ㅝ') },
            { NEUTRAL_VOWEL.IndexOf('ㅔ'), NEUTRAL_VOWEL.IndexOf('ㅞ') },
            { NEUTRAL_VOWEL.IndexOf('ㅣ'), NEUTRAL_VOWEL.IndexOf('ㅟ') }
        },
        new Dictionary<int, int>()
        {
            { NEUTRAL_VOWEL.IndexOf('ㅣ'), NEUTRAL_VOWEL.IndexOf('ㅢ') }
        }
    };

    private enum FirstVowel
    {
        ㅗ = 8,
        ㅜ = 13,
        ㅡ = 18,
    }

    private static int CombineTwoVowel(int first, int second)
    {
        int outputValue = first;

        switch ((FirstVowel)first)
        {
            case FirstVowel.ㅗ:
                first = 0;
                break;

            case FirstVowel.ㅜ:
                first = 1;
                break;

            case FirstVowel.ㅡ:
                first = 2;
                break;

            default:
                return outputValue;
        }

        if (DOUBLE_VOWEL_INDEX[first].ContainsKey(second))
            outputValue = DOUBLE_VOWEL_INDEX[first][second];

        return outputValue;
    }

    #endregion

#endregion
}