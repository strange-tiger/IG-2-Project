using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tu5_minerJ1 : MonoBehaviour
{
    [SerializeField] private Slider _minerJSlider;
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private bool _isFirst;

    void Update()
    {
        if (_minerJSlider.value >= 1 && !_isFirst)
        {
            ++_lobby1TutorialsQuest.AdvanceQuest;
            _isFirst = true;
        }
    }

    private void OnEnable()
    {
        _isFirst = false;
    }
}
