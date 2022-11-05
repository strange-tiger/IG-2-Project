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

    private int _luncherSettingCount = 0;
    private int _luncherTupeCount = 0;

    [SerializeField] private GameObject[] _shootingObjectPrefabs;

    /// <summary>
    /// [플레이어 쪽, 왼쪽][플레이어 쪽, 오른쪽][앞 쪽, 왼쪽][앞 쪽, 오른쪽]
    /// </summary>
    private int _luncherBit = 0b_1000;

    [SerializeField] private ObjectLuncher[] _lunchers = new ObjectLuncher[_MAX_LUNCHER_COUNT];
    private const int _MAX_LUNCHER_COUNT = 4;

    public void SetLuncher(float playTime)
    {
        _luncherTimmings[_luncherTimmings.Length - 2].LimitTime = playTime;
        _luncherTimmings[_luncherTimmings.Length - 1].LimitTime = playTime + 5;
    }

    public void LunchObject(int gameTime)
    {
        int objectNumber = UnityEngine.Random.Range(0, _shootingObjectPrefabs.Length);

        if(gameTime >= _luncherTimmings[_luncherSettingCount].LimitTime)
        {
            ++_luncherSettingCount;
            if(_luncherSettingCount == _luncherTimmings.Length)
            {
                return;
            }
            _luncherTupeCount = 0;
        }

        foreach(ObjectLuncher luncher in _lunchers)
        {
            luncher.GetRandomDegreeInRange(
                (int)_luncherTimmings[_luncherSettingCount].LuncherType[_luncherTupeCount],
                _shootingObjectPrefabs[objectNumber]);
        }

        Debug.Log(gameTime);
        _luncherTupeCount = (_luncherTupeCount + 1) % _luncherTimmings[_luncherSettingCount].LuncherType.Length;
    }
}
