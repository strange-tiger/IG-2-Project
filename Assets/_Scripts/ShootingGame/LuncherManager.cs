using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuncherManager : MonoBehaviour
{
    [Serializable]
    public class LuncherTimingSetting
    {
        [SerializeField] private float _limitTime;
        public float LimitTime { get => _limitTime; set => _limitTime = value; }
        [SerializeField] private ELuncherId[] _luncherType;
        public ELuncherId[] LuncherType { get => _luncherType; set => _luncherType = value; }
    }

    public enum ELuncherId {
        FrontLeft = 0b_0001,
        FrontRight = 0b_0010,
        BackLeft = 0b_0100,
        BackRight = 0b_1000,

        Lefts = 0b_0101,
        Rights = 0b_1010,

        Fronts = 0b_0011,
        Backs = 0b_1100,

        All = 0b_1111,
    }

    [SerializeField] private LuncherTimingSetting[] _luncherTimmings;
    public LuncherTimingSetting[] LuncherTimmings { get => _luncherTimmings; set => _luncherTimmings = value; }

    [SerializeField] private float _lunchOffsetSeconds = 1f;

    private float _elapsedTime = 1f;
    private float _gameTime = 0f;

    private int _luncherSettingCount = 0;

    private bool _isStarted = false;
    private bool _isEnd = false;
    /// <summary>
    /// [플레이어 쪽, 왼쪽][플레이어 쪽, 오른쪽][앞 쪽, 왼쪽][앞 쪽, 오른쪽]
    /// </summary>
    private int _luncherBit = 0b_1000;

    [SerializeField] private ObjectLuncher[] _lunchers = new ObjectLuncher[_MAX_LUNCHER_COUNT];
    private const int _MAX_LUNCHER_COUNT = 4;

    private void Awake()
    {
        _elapsedTime = _lunchOffsetSeconds;
    }

    public void SetLuncher(float playTime)
    {
        _luncherTimmings[_luncherTimmings.Length - 2].LimitTime = playTime;
        _luncherTimmings[_luncherTimmings.Length - 1].LimitTime = playTime + 5;
        _isStarted = true;
    }

    private void Update()
    {
        if(!_isStarted || !_isEnd)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;
        _gameTime += Time.deltaTime;
        if (_elapsedTime >= _lunchOffsetSeconds)
        {
            _elapsedTime -= _lunchOffsetSeconds;
            if(_gameTime >= _luncherTimmings[_luncherSettingCount].LimitTime)
            {

                ++_luncherSettingCount;
                //if(_lucherSetingCount)
            }
        }
    }
}
