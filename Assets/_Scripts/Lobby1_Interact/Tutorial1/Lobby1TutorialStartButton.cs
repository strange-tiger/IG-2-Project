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

    private bool isButton;
    private bool isOn = true;

    private void Start()
    {
        for (int i = 0; i < _tutorialButton.Length; ++i)
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

        _tutorialButton[6].onClick.RemoveListener(OnExitButton);
        _tutorialButton[6].onClick.AddListener(OnExitButton);

        OnButtonAction = OnButtons;
    }

    private void Update()
    {
        Debug.Log(isOn);

        if (_tutorialController.DialogueNum == 3 && !isButton)
        {
            OnButtonAction?.Invoke();
            isButton = true;
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnClickButton(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnClickButton(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnClickButton(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnClickButton(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnClickButton(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            OnClickButton(5);
        }
#endif
    }

    private void OnClickButton(int num)
    {
        for (int i = 0; i > _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                isOn = false;
                return;
            }
        }

        if (isOn)
        {
            switch (num)
            {
                case 0:
                    _tutorialController.QuestAcceptEvent.Invoke(3);
                    break;
                case 1:
                    _tutorialController.QuestAcceptEvent.Invoke(6);
                    break;
                case 2:
                    _tutorialController.QuestAcceptEvent.Invoke(10);
                    break;
                case 3:
                    _tutorialController.QuestAcceptEvent.Invoke(17);
                    break;
                case 4:
                    _tutorialController.QuestAcceptEvent.Invoke(24);
                    break;
                case 5:
                    _tutorialController.QuestAcceptEvent.Invoke(36);
                    break;
                default:
                    break;
            }
            Debug.Log("이게 또나오면 안되는데..");
            _tutorialObject[num].SetActive(true);
            _tutorialButton[num].interactable = false;
        }
    }

    private void OnButtons()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            _tutorialButton[i].interactable = true;
        }
    }

    private void OnExitButton()
    {
        for (int i = 0; i > _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                _tutorialObject[i].SetActive(false);
                return;
            }
        }
    }

    private void OnDisable()
    {
        _tutorialButton[6].onClick.RemoveListener(OnExitButton);
    }
}
