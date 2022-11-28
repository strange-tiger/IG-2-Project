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

    private bool asdasd;

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

        _tutorialButton[6].onClick.RemoveListener(OnExitButton);
        _tutorialButton[6].onClick.AddListener(OnExitButton);

        OnButtonAction = OnButtons;
    }

    private void Update()
    {
        if (_tutorialController.DialogueNum == 3 && !asdasd)
        {
            OnButtonAction?.Invoke();
            asdasd = true;
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

#endif
    }

    private void OnClickButton(int num)
    {
        for (int i = 0; i > _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                return;
            }
        }

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

        _tutorialObject[num].SetActive(true);
        _tutorialButton[num].interactable = false;
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
