using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLuncher : MonoBehaviour
{

    [SerializeField] private float _minXDegree = -15f;
    [SerializeField] private float _maxXDegree = 30f;

    [SerializeField] private float _minYDegree = -10f;
    [SerializeField] private float _maxYDegree = 10f;

    [SerializeField] private int _maxLunchCount = 3;

    [SerializeField] private Vector3 _originalDegree = new Vector3(0f, 0f, 0f);

    [SerializeField] private LuncherManager.ELuncherId _luncherId;
    private int _luncherIdInt;
    public LuncherManager.ELuncherId LuncherId { get => _luncherId; set => _luncherId = value; }

    [SerializeField] private Transform _lunchPointTransform;
    [SerializeField] private Transform _objectParent;

    private void Awake()
    {
        _luncherIdInt = (int)_luncherId;
    }

    public void GetRandomDegreeInRange(int _lunchCode, GameObject prefab)
    {
        if((_lunchCode & _luncherIdInt) == 0)
        {
            return;
        }

        for(int i = 0; i<_maxLunchCount; ++i)
        {
            float xDegree = Random.Range(_minXDegree, _maxXDegree);
            float zDegree = Random.Range(_minYDegree, _maxYDegree);

            transform.rotation = Quaternion.Euler(_originalDegree);
            transform.rotation = Quaternion.Euler(xDegree + _originalDegree.x, zDegree + _originalDegree.y, _originalDegree.z);

            GameObject newObject = Instantiate(prefab, _lunchPointTransform.position, 
                Quaternion.Euler(xDegree + _originalDegree.x, zDegree + _originalDegree.y, _originalDegree.z));
            newObject.transform.parent = _objectParent;
        }
    }
}