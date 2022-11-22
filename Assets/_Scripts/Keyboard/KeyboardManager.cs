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

    private static readonly Vector3 MOVE_KEYBOARD = new Vector3(0f, -60f, -10f);
    public static void OpenKeyboard()
    {
        _inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
        _typedText.gameObject.SetActive(true);

        _keyboard.localPosition = MOVE_KEYBOARD;

        ChangeLayout(EKeyboardLayout.QWERTY);
    }

    public static void OpenKeyboard(EKeyboardLayout type)
    {
        _inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

        _keyboard.localPosition = MOVE_KEYBOARD;

        _typedText.gameObject.SetActive(true);

        ChangeLayout(type);
    }

    private static void ChangeLayout(EKeyboardLayout type)
    {
        CloseLayout();

        _currentLayout = type;
        _layouts[(int)_currentLayout].SetActive(true);
    }

    public static void CloseKeyboard()
    {
        CloseLayout();
        _inputField = null;
        PressClear();
        _typedText.gameObject.SetActive(false);
    }

    private static void CloseLayout()
    {
        foreach (GameObject layout in _layouts)
        {
            layout.SetActive(false);
        }
    }

    public static void PressKey()
    {
        CutKoreanType();

        _typedText.text += EventSystem.current.currentSelectedGameObject.name;

        EventSystem.current.SetSelectedGameObject(null);
    }

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

    private static void CutKoreanType()
    {
        if (s_isKorean)
        {
            s_isKorean = !s_isKorean;

            s_prevSentence = string.Empty;
            s_koreanSentence = string.Empty;
        }
    }

    public static void PressSpace()
    {
        CutKoreanType();

        _typedText.text += " ";

        EventSystem.current.SetSelectedGameObject(null);
    }

    public static void PressBackspace()
    {
        CutKoreanType();

        if (_typedText.text.Length == 0) return;
        _typedText.text = _typedText.text.Substring(0, _typedText.text.Length - 1);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public static void PressClear()
    {
        CutKoreanType();

        if (_typedText.text.Length == 0) return;
        _typedText.text = string.Empty;
    }

    public static void PressShift()
    {
        CutKoreanType();

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

    public static void Submit()
    {
        _inputField.text = _typedText.text;
        CloseKeyboard();
    }

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

    #region 廃越_脊径

    private const string INITIAL_CONSONANT = "ぁあいぇえぉけげこさざしじすずせぜそぞ";
    private const string NEUTRAL_VOWEL = "ただちぢっつづてでとどなにぬねのはばぱひび";
    private const string FINAL_CONSONANT = "ぁあぃいぅうぇぉおかがきぎくぐけげごさざしじずせぜそぞ";

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
                            middleIndex = CombineTwoConsonant(middleIndex, finalVowelIndex);

                            finalConsonantIndex = 0;

                            int i = CalculateLetterCode(initialIndex, middleIndex, finalConsonantIndex);
                            char letter = Convert.ToChar(i);
                            output += letter.ToString();

                            index += 3;
                        }
                        else if (initialIndex != -1 && middleIndex != -1 && finalConsonantIndex != -1)
                        {
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

    #region 戚掻切乞製

    private static readonly Dictionary<int, int>[] DOUBLE_CONSONANT_INDEX = new Dictionary<int, int>[4]
    {
        new Dictionary<int, int>()
        {
            { FINAL_CONSONANT.IndexOf('さ'), FINAL_CONSONANT.IndexOf('ぃ') }
        },
        new Dictionary<int, int>()
        {
            { FINAL_CONSONANT.IndexOf('じ'), FINAL_CONSONANT.IndexOf('ぅ') },
            { FINAL_CONSONANT.IndexOf('ぞ'), FINAL_CONSONANT.IndexOf('う') }
        },
        new Dictionary<int, int>()
        {
            { FINAL_CONSONANT.IndexOf('ぁ'), FINAL_CONSONANT.IndexOf('お') },
            { FINAL_CONSONANT.IndexOf('け'), FINAL_CONSONANT.IndexOf('か') },
            { FINAL_CONSONANT.IndexOf('げ'), FINAL_CONSONANT.IndexOf('が') },
            { FINAL_CONSONANT.IndexOf('さ'), FINAL_CONSONANT.IndexOf('き') },
            { FINAL_CONSONANT.IndexOf('ぜ'), FINAL_CONSONANT.IndexOf('ぎ') },
            { FINAL_CONSONANT.IndexOf('ぞ'), FINAL_CONSONANT.IndexOf('ぐ') }
        },
        new Dictionary<int, int>()
        {
            { FINAL_CONSONANT.IndexOf('さ'), FINAL_CONSONANT.IndexOf('ご') }
        }
    };

    private enum FirstConsonant
    {
        ぁ = 0,
        い = 3,
        ぉ = 7,
        げ = 16
    }

    private static int CombineTwoConsonant(int first, int second)
    {
        int outputValue = first;

        switch ((FirstConsonant)first)
        {
            case FirstConsonant.ぁ:
                break;

            case FirstConsonant.い:
                first = 1;
                break;

            case FirstConsonant.ぉ:
                first = 2;
                break;

            case FirstConsonant.げ:
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
            { NEUTRAL_VOWEL.IndexOf('た'), NEUTRAL_VOWEL.IndexOf('と') },
            { NEUTRAL_VOWEL.IndexOf('だ'), NEUTRAL_VOWEL.IndexOf('ど') },
            { NEUTRAL_VOWEL.IndexOf('び'), NEUTRAL_VOWEL.IndexOf('な') }
        },
        new Dictionary<int, int>()
        {
            { NEUTRAL_VOWEL.IndexOf('っ'), NEUTRAL_VOWEL.IndexOf('ね') },
            { NEUTRAL_VOWEL.IndexOf('つ'), NEUTRAL_VOWEL.IndexOf('の') },
            { NEUTRAL_VOWEL.IndexOf('び'), NEUTRAL_VOWEL.IndexOf('は') }
        },
        new Dictionary<int, int>()
        {
            { NEUTRAL_VOWEL.IndexOf('び'), NEUTRAL_VOWEL.IndexOf('ひ') }
        }
    };

    private enum FirstVowel
    {
        で = 8,
        ぬ = 13,
        ぱ = 18,
    }

    private static int CombineTwoVowel(int first, int second)
    {
        int outputValue = first;

        switch ((FirstVowel)first)
        {
            case FirstVowel.で:
                first = 0;
                break;

            case FirstVowel.ぬ:
                first = 1;
                break;

            case FirstVowel.ぱ:
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