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
    private float _originalZRotation = 90f;
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

        _playerTransform = transform.root;

        SetPlayerPositionOnMap();
    }

    private void SetPlayerPositionOnMap()
    {
        Vector3 relativePosition = _playerTransform.position - _mapCenterPivot;

        float playerSpriteXPosition = _mapUIWidth / _mapWidth * relativePosition.x;
        playerSpriteXPosition = Mathf.Clamp(playerSpriteXPosition, -_mapUIWidth / 2, _mapUIWidth / 2);

        float playerSpriteZPosition = _mapUIHeight / _mapHeight * relativePosition.z;
        playerSpriteZPosition = Mathf.Clamp(playerSpriteZPosition, -_mapUIHeight / 2, _mapUIHeight / 2);

        _playerSprite.transform.localPosition = new Vector3(playerSpriteXPosition, playerSpriteZPosition, 0f);
    }

    private void SetPlayerRotationOnMap()
    {
        Debug.Log($"SetPlayerRotation {_playerTransform.eulerAngles.y}");

        _playerSprite.transform.eulerAngles = new Vector3(0f, 0f, _playerTransform.eulerAngles.y + _originalZRotation);
        _playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, _playerSprite.transform.rotation.z);
    }

    private void Update()
    {
        if(_isFixedPosition)
        {
            return;
        }

        SetPlayerRotationOnMap();
    }

    private void OnDisable()
    {
        foreach(Toggle toggle in _toggleList)
        {
            toggle.isOn = false;
        }
    }
}
