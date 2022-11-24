using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby1TutorialStartButton : MonoBehaviour
{
    [SerializeField] private Button[] _tutorialButton;
    [SerializeField] private GameObject[] tutorialObject;

    private void Start()
    {
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
        GameObject[] obj = tutorialObject;
        foreach (bool value in obj)
        {
            if (value)
            {
                return;
            }
        }

        tutorialObject[num].SetActive(true);
        _tutorialButton[num].interactable = false;
    }
}
