using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaScoreboardUI : MonoBehaviour
{
    [SerializeField] private GroupManager _groupManager;

    [Header("�����ڵ� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI[] _championNameText;

    [Header("�����ڵ� Hp �����̵�")]
    [SerializeField] private Slider[] _championSlider;

    [Header("���� �����ڵ� Hp")]
    [SerializeField] private AIDamage[] _championHp;

    [Header("Ÿ�̸� �ؽ�Ʈ")]
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
    /// Ÿ�̸�
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
    /// Ÿ�̸� �۾� ���
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
