using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Lobby1TutorialStartButton : MonoBehaviour
{
    [SerializeField] private Button[] _tutorialButton;
    [SerializeField] private GameObject[] _tutorialObject;

    public UnityEvent QuestClear = new UnityEvent();

    private void Start()
    {
        QuestClear.RemoveListener(Clear);
        QuestClear.AddListener(Clear);

        for (int i = 0; i < _tutorialButton.Length; ++i)
        {
            int num = i;
            _tutorialButton[i].onClick.RemoveAllListeners();
            _tutorialButton[i].onClick.AddListener(() =>
            {
                OnClickButton(num);
            });
        }
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

        _tutorialObject[num].SetActive(true);
        _tutorialButton[num].interactable = false;
    }

    private void Clear()
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

    private void OnEnable()
    {
        QuestClear.RemoveListener(Clear);
    }
}
