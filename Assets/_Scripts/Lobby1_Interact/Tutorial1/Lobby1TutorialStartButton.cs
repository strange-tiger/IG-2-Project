using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Lobby1TutorialStartButton : MonoBehaviour
{
    [SerializeField] private Button[] _tutorialButton;
    [SerializeField] private GameObject[] _tutorialObject;
    [SerializeField] private TutorialController _tutorialController;

    private Action OnButtonAction;

    private bool _isButton;
    private bool _isOn = true;
    private bool _isQuest;


    private bool _isOne;
    private bool _isTwo;
    private bool _isThree;
    private bool _isFour;
    private bool _isFive;
    private bool _isSix;

    private void Start()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            _tutorialButton[i].interactable = false;
        }

        for (int i = 0; i < _tutorialButton.Length; ++i)
        {
            int num = i;
            _tutorialButton[i].onClick.RemoveAllListeners();
            _tutorialButton[i].onClick.AddListener(() =>
            {
                OnClickButton(num);
            });
        }

        _tutorialButton[6].onClick.RemoveListener(ClickExitButton);
        _tutorialButton[6].onClick.AddListener(ClickExitButton);

        OnButtonAction = OnButtons;
    }

    private void Update()
    {
        if (_tutorialController.DialogueNum == 3 && !_isButton)
        {
            OnButtonAction?.Invoke();
            _isButton = true;
        }
        #region input
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnClickButton(0);
            Debug.Log("1번누름");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnClickButton(1);
            Debug.Log("2번누름");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnClickButton(2);
            Debug.Log("3번누름");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnClickButton(3);
            Debug.Log("4번누름");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnClickButton(4);
            Debug.Log("5번누름");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            OnClickButton(5);
            Debug.Log("6번누름");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClickExitButton();
        }
#endif
        #endregion

        if (_isOne)
        {
            if (_tutorialController.DialogueNum == 5 && !_isQuest)
            {
                _tutorialController.IsTutorialQuest = true;
                _isQuest = true;
            }

            if (_tutorialController.DialogueNum == 6)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isOne = false;
                ClickExitButton();
            }
        }

        if (_isTwo)
        {
            if (_tutorialController.DialogueNum == 10)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isTwo = false;
                ClickExitButton();
            }
        }

        if (_isThree)
        {
            if (_tutorialController.DialogueNum == 17)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isThree = false;
                ClickExitButton();
            }
        }

        if (_isFour)
        {
            if (_tutorialController.DialogueNum == 24)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isFour = false;
                ClickExitButton();
            }
        }

        if (_isFive)
        {
            if (_tutorialController.DialogueNum == 36)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isFive = false;
                ClickExitButton();
            }
        }

        if (_isSix)
        {
            if (_tutorialController.DialogueNum == 54)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isSix = false;
                ClickExitButton();
            }
        }
        
    }

    private void OnClickButton(int num)
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                _isOn = false;
                return;
            }
        }

        if (_isOn)
        {
            switch (num)
            {
                case 0:
                    _tutorialController.QuestAcceptEvent.Invoke(3);
                    _isOne = true;
                    break;
                case 1:
                    _tutorialController.QuestAcceptEvent.Invoke(6);
                    _isTwo = true;
                    break;
                case 2:
                    _tutorialController.QuestAcceptEvent.Invoke(10);
                    _isThree = true;
                    break;
                case 3:
                    _tutorialController.QuestAcceptEvent.Invoke(17);
                    _isFour = true;
                    break;
                case 4:
                    _tutorialController.QuestAcceptEvent.Invoke(24);
                    _isFive = true;
                    break;
                case 5:
                    _tutorialController.QuestAcceptEvent.Invoke(36);
                    _isSix = true;
                    break;
                default:
                    break;
            }
            
            _tutorialObject[num].SetActive(true);
            _tutorialButton[num].interactable = false;
            ExitButton();
        }

    }

    private void OnButtons()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            _tutorialButton[i].interactable = true;
        }
    }

    private void ExitButton()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                _tutorialButton[6].interactable = true;
                return;
            }
        }
    }

    private void ClickExitButton()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                _tutorialObject[i].SetActive(false);
                return;
            }
        }
        _isOn = true;
        _tutorialController.IsTutorialQuest = false;
    }

    private void OnDisable()
    {
        _tutorialButton[6].onClick.RemoveListener(ClickExitButton);
    }
}
