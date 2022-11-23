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

    [Header("Icons")]
    [SerializeField] private int _iconCounts = 10;
    [SerializeField] private Transform _onMapIcons;
    [SerializeField] private Transform _onMapMarkers;
    [SerializeField] private Transform _mapToggleIcons;
    private List<Toggle> _toggleList = new List<Toggle>();

    private void Awake()
    {
        _mapWidth = Mathf.Abs(_mapPositionPivot[0].x - _mapPositionPivot[1].x);
        _mapHeight = Mathf.Abs(_mapPositionPivot[0].z - _mapPositionPivot[1].z);

        SetToggles();
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

    public void SetFixedPlayerPosition(Vector3 fixedPosition, Vector3 fixedRotation)
    {
        _isFixedPosition = true;

        _playerSprite.transform.localPosition = fixedPosition;
        _playerSprite.transform.localRotation = Quaternion.Euler(fixedRotation);

        Debug.Log(fixedPosition + " " + fixedRotation);
    }

    private void OnEnable()
    {
        if(_isFixedPosition)
        {
            return;
        }
        
        SetPlayerPositionOnMap();
    }

    private void SetPlayerPositionOnMap()
    {
        Debug.Log("SetPlayerPosition");
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
