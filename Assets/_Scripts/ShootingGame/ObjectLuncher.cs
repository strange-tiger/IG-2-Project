using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLuncher : MonoBehaviour
{

    [SerializeField] private float _minXDegree = -15f;
    [SerializeField] private float _maxXDegree = 30f;

    [SerializeField] private float _minZDegree = -10f;
    [SerializeField] private float _maxZDegree = 10f;

    [SerializeField] private Vector3 _originalDegree = new Vector3(0f, 0f, 0f);

    [SerializeField] private LuncherManager.ELuncherId _luncherId;
    private int _luncherIdInt;
    public LuncherManager.ELuncherId LuncherId { get => _luncherId; set => _luncherId = value; }

    private void Awake()
    {
        _luncherIdInt = (int)_luncherId;
    }

    public void GetRandomDegreeInRange(int _lunchCode)
    {
        if((_lunchCode & _luncherIdInt) == 0)
        {
            return;
        }

        float xDegree = Random.Range(_minXDegree, _maxXDegree);
        float zDegree = Random.Range(_minZDegree, _maxZDegree);

        transform.rotation = Quaternion.Euler(_originalDegree);
        transform.rotation = Quaternion.Euler(xDegree + _originalDegree.x, zDegree + _originalDegree.y, _originalDegree.z);
    }
}