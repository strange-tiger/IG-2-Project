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
    //[SerializeField] private Defines.EMapType _mapType = Defines.EMapType.None;
    //
    //[SerializeField] private bool _isFixedPosition = false;
    //[SerializeField] private Vector3 _fixedPosition = Vector3.zero;
    //[SerializeField] private Vector3 _fixedRotation = Vector3.zero;

    private GameObject _currentMap;

    public void SetMap(Defines.EMapType mapType)
    {
        _currentMap = _maps[(int)mapType];
        _currentMap.SetActive(true);
    }

    public void SetFixedPlayerPosition(Vector3 fixedPosition, Vector3 fixedRotation)
    {
        LobbyMapManager mapManager = _currentMap.GetComponent<LobbyMapManager>();
        mapManager.SetFixedPlayerPosition(fixedPosition, fixedRotation);
    }
}