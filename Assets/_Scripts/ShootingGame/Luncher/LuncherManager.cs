using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuncherManager : MonoBehaviour
{
    // 발사대 관련
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
    public enum EShootingObject
    {
        Bottle,
        Tin,
        Barrel,
        Skull,
    }

    [Header("Luncher")]
    [SerializeField] private ObjectLuncher[] _lunchers = new ObjectLuncher[_MAX_LUNCHER_COUNT];
    private const int _MAX_LUNCHER_COUNT = 4;

    [Header("Luncher Timing")]
    [SerializeField] private LuncherTimingSetting[] _luncherTimings;
    public LuncherTimingSetting[] LuncherTimings { get => _luncherTimings; set => _luncherTimings = value; }
    private LuncherTimingSetting _currentLuncherTimingSetting;
    private int _luncherSettingCount = 0;
    private int _luncherTypeCount = 0;

    // 발사 오브젝트 관련
    [Serializable]
    public class ShootingObjectRate
    {
        [SerializeField] private EShootingObject _shootingObjectType;
        public EShootingObject ShootingObjectType { get => _shootingObjectType; set => _shootingObjectType = value; }
        [SerializeField] private float _objectRate;
        public float ObjectRate { get => _objectRate; set => _objectRate = value; }
    }
    [Serializable]
    public class ShootingObjectTiming
    {
        [SerializeField] private float _limitTime;
        public float LimitTime { get => _limitTime; set => _limitTime = value; }
        [SerializeField] private ShootingObjectRate[] _shootingObjects;
        public ShootingObjectRate[] ShootingObjects { get => _shootingObjects; set => _shootingObjects = value; }
    }

    [Header("ShootingObject Timing")]
    [SerializeField] ShootingObjectTiming[] _shootingObjectTimings;
    private ShootingObjectTiming _currentShootingObjectTiming;
    private int _shootingObjectCount = 0;
    private float _shootingObjectMaxRate = 0;

    public void SetLuncher(float playTime)
    {
        _luncherTimings[_luncherTimings.Length - 2].LimitTime = playTime;
        _luncherTimings[_luncherTimings.Length - 1].LimitTime = playTime + 5;
        _currentLuncherTimingSetting = _luncherTimings[0];

        for(int i = 0; i < 6; ++i)
        {
            _shootingObjectTimings[_shootingObjectTimings.Length - (6 - i)].LimitTime = playTime + i;
        }
        _currentShootingObjectTiming = _shootingObjectTimings[0];
    }

    public void LunchObject(int gameTime)
    {
        if(gameTime > _currentLuncherTimingSetting.LimitTime)
        {
            ++_luncherSettingCount;
            if(_luncherSettingCount == _luncherTimings.Length)
            {
                return;
            }
            _currentLuncherTimingSetting = _luncherTimings[_luncherSettingCount];

            _luncherTypeCount = 0;
        }

        if(gameTime > _currentShootingObjectTiming.LimitTime)
        {
            ++_shootingObjectCount;
            if(_shootingObjectCount == _shootingObjectTimings.Length)
            {
                return;
            }
            _currentShootingObjectTiming = _shootingObjectTimings[_shootingObjectCount];

            _shootingObjectMaxRate = 0;
            foreach(ShootingObjectRate objectRate in _shootingObjectTimings[_shootingObjectCount].ShootingObjects)
            {
                _shootingObjectMaxRate += objectRate.ObjectRate;
            }
        }

        EShootingObject lunchObject = SelectObject();

        foreach(ObjectLuncher luncher in _lunchers)
        {
            luncher.LunchObjects(
                (int)_currentLuncherTimingSetting.LuncherType[_luncherTypeCount],
                lunchObject);
        }

        Debug.Log(gameTime);
        _luncherTypeCount = (_luncherTypeCount + 1) % _currentLuncherTimingSetting.LuncherType.Length;
    }

    private EShootingObject SelectObject()
    {
        ShootingObjectTiming shootingTiming = _shootingObjectTimings[_shootingObjectCount];

        float randomRate = UnityEngine.Random.Range(0f, _shootingObjectMaxRate);

        float rate = 0f;
        foreach(ShootingObjectRate objectRate in shootingTiming.ShootingObjects)
        {
            rate += objectRate.ObjectRate;
            if(randomRate < rate)
            {
                return objectRate.ShootingObjectType;
            }
        }

        return shootingTiming.ShootingObjects[shootingTiming.ShootingObjects.Length - 1].ShootingObjectType;
    }
}
