using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaScoreboardUI : MonoBehaviour
{
    [SerializeField] private GroupManager _groupManager;

    [Header("참가자들 텍스트")]
    [SerializeField] private TextMeshProUGUI[] _championNameText;

    [Header("참가자들 Hp 슬라이드")]
    [SerializeField] private Slider[] _championSlider;

    [Header("얻어올 참가자들 Hp")]
    [SerializeField] private AIDamage[] _championHp;

    [Header("타이머 텍스트")]
    [SerializeField] private TextMeshProUGUI _timeText;

    private float[] _hp = new float[4];

    private int _minute = 15;
    private int _second = 0;
    private float _cumulativeTime;

    private void Start()
    {
        for (int i = 0; i < _championNameText.Length; ++i)
        {
            _championNameText[i].text = _groupManager.transform.GetChild(i).name;
        }

        for (int i = 0; i < _championSlider.Length; ++i)
        {
            _hp[i] = _championHp[i].Hp;
        }
    }

    private void Update()
    {
        FlowingTime();
        UpdateTimerText(_minute, _second);
        SetChampionHp();
    }

    /// <summary>
    /// 타이머
    /// </summary>
    private void FlowingTime()
    {
        _cumulativeTime += Time.deltaTime;

        if (_cumulativeTime > 1f)
        {
            if (_second == 0)
            {
                _second = 59;
                --_minute;
            }
            else
            {
                --_second;
            }
            _cumulativeTime = 0f;
        }

        if (_minute < 0)
        {
            _minute = 0;
            _second = 0;

            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// 타이머 글씨 출력
    /// </summary>
    /// <param name="minute"></param>
    /// <param name="second"></param>
    private void UpdateTimerText(int minute, int second)
    {
        _timeText.text = $"{minute} : {second:D2}";
    }

    private void SetChampionHp()
    {
        for (int i = 0; i < _championSlider.Length; ++i)
        {
            _championSlider[i].value = _championHp[i].Hp / _hp[i] * 100;
        }
    }
}
