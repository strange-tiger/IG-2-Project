using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMapManager : MonoBehaviour
{
    [Header("Map Pivot")]
    [SerializeField] private Vector3[] _mapPositionPivot = new Vector3[2];
    private Vector3 _mapCenterPivot;
    private float _mapWidth;
    private float _mapHeight;

    private float _mapUIWidth;
    private float _mapUIHeight;

    [Header("Player")]
    [SerializeField] private GameObject _playerSprite;
    private Transform _playerTransform;
    private bool _isFixedPosition;

    [Header("Icons")]
    [SerializeField] private int _iconCounts = 10;
    [SerializeField] private Transform _onMapIcons;
    [SerializeField] private Transform _onMapMarkers;
    [SerializeField] private Transform _mapToggleIcons;
    private List<Toggle> _toggleList = new List<Toggle>();

    private void Awake()
    {
        _mapCenterPivot = new Vector3(
            (_mapPositionPivot[0].x + _mapPositionPivot[1].x) / 2, 0f,
            (_mapPositionPivot[0].z + _mapPositionPivot[1].z) / 2);

        _mapWidth = Mathf.Abs(_mapPositionPivot[0].x - _mapPositionPivot[1].x);
        _mapHeight = Mathf.Abs(_mapPositionPivot[0].z - _mapPositionPivot[1].z);

        RectTransform rectTransfrom = GetComponent<RectTransform>();
        _mapUIWidth = rectTransfrom.rect.width;
        _mapUIHeight = rectTransfrom.rect.height;

        _playerTransform = transform.parent;

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
        SetPlayerRotationOnMap();
    }

    private void SetPlayerPositionOnMap()
    {
        Debug.Log("SetPlayerPosition");

        Vector3 relativePosition = _playerTransform.position - _mapCenterPivot;

        float playerSpriteXPosition = _mapUIWidth / _mapWidth * relativePosition.x;
        float playerSpriteZPosition = _mapUIHeight / _mapHeight * relativePosition.z;

        _playerSprite.transform.localPosition = new Vector3(playerSpriteXPosition, 0f, playerSpriteZPosition);
    }

    private void SetPlayerRotationOnMap()
    {
        _playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, _playerTransform.rotation.z);
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
