using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defines
{
    public enum EMapType
    {
        None,
        Lobby1,
        Lobby2,
    }
}

public class MapPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _maps;
    [SerializeField] private Defines.EMapType _mapType = Defines.EMapType.None;

    [SerializeField] private bool _isFixedPosition = false;
    [SerializeField] private Vector3 _fixedPosition = Vector3.zero;
    [SerializeField] private Vector3 _fixedRotation = Vector3.zero;

    private void Awake()
    {
        GameObject map = _maps[(int)_mapType];

        map.SetActive(true);
        if(_mapType != Defines.EMapType.None && _isFixedPosition)
        {
            LobbyMapManager mapManager = map.GetComponent<LobbyMapManager>();
            mapManager.IsFixedPosition = _isFixedPosition;
            mapManager.FixedPosition = _fixedPosition;
            mapManager.FixedRotation = _fixedRotation;
        }
    }
}