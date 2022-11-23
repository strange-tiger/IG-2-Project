using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMapManager : MonoBehaviour
{
    [Header("Map Pivot")]
    [SerializeField] private Vector3[] _mapPositionPivot = new Vector3[2];
    private float _mapWidth;
    private float _mapHeight;

    [Header("Player")]
    [SerializeField] private GameObject _playerSprite;

    private bool _isFixedPosition;
    public bool IsFixedPosition 
    { 
        get => _isFixedPosition; 
        set => _isFixedPosition = value; 
    }
    
    private Vector3 _fixedPosition = Vector3.zero;
    public Vector3 FixedPosition 
    { 
        get => _fixedPosition; 
        set => _fixedPosition = value; 
    }

    private Vector3 _fixedRotation = Vector3.zero;
    public Vector3 FixedRotation
    {
        get => _fixedRotation;
        set => _fixedRotation = value;
    }

    [Header("Icons")]
    [SerializeField] private int _iconCounts = 10;
    [SerializeField] private Transform _onMapIcons;
    [SerializeField] private Transform _onMapMarkers;
    [SerializeField] private Transform _mapToggleIcons;
    private List<Toggle> _toggleList;

    private void Awake()
    {
        _mapWidth = Mathf.Abs(_mapPositionPivot[0].x - _mapPositionPivot[1].x);
        _mapHeight = Mathf.Abs(_mapPositionPivot[0].z - _mapPositionPivot[1].z);

        SetToggles();
    }

    private void OnEnable()
    {
        if(_isFixedPosition)
        {
            _playerSprite.transform.localPosition = _fixedPosition;
            _playerSprite.transform.localRotation = Quaternion.Euler(_fixedRotation);
        }
        else
        {
            SetPlayerPositionOnMap();
        }
    }

    private void SetToggles()
    {
        for (int i = 0; i < _iconCounts; ++i)
        {
            GameObject icon = _onMapIcons.GetChild(i).gameObject;
            GameObject marker = _onMapMarkers.GetChild(i).gameObject;

            Toggle toggle = _mapToggleIcons.GetChild(i).GetComponentInChildren<Toggle>();
            toggle.onValueChanged.AddListener((bool value) =>
           {
               icon.SetActive(value);
               marker.SetActive(value);
           });
            toggle.isOn = false;

            _toggleList.Add(toggle);
        }
    }

    private void SetPlayerPositionOnMap()
    {

    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        foreach(Toggle toggle in _toggleList)
        {
            toggle.isOn = false;
        }
    }
}
