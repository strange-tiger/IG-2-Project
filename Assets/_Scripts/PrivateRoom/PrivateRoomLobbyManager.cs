using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapType = Defines.EMapType;
using SceneNumber = Defines.ESceneNumber;

public class PrivateRoomLobbyManager : LobbyChanger
{
    [Serializable]
    public class FixedMapInfo
    {
        [SerializeField] private Vector3 _fixedPosition;
        public Vector3 FixedPosition
        {
            get => _fixedPosition;
            set => _fixedPosition = value;
        }

        [SerializeField] private Vector3 _fixedRotation;
        public Vector3 FixedRotation
        {
            get => _fixedRotation;
            set => _fixedRotation = value;
        }
    }

    [SerializeField] private FixedMapInfo[] _fixedMapInfos;

    protected override void Awake()
    {
        if(PlayerPrefs.HasKey("PrevScene"))
        {
            int prevScene = PlayerPrefs.GetInt("PrevScene");
            Debug.Log($"prevScene: {prevScene} {prevScene >= (int)SceneNumber.FantasyLobby && prevScene <= (int)SceneNumber.WesternLobby}");

            if(prevScene >= (int) SceneNumber.FantasyLobby &&
                prevScene <= (int) SceneNumber.WesternLobby)
            {
                int mapinfoNumber = prevScene - (int)SceneNumber.FantasyLobby;

                _mapType = (MapType)(mapinfoNumber + 1);
                _isFixedPosition = true;

                _fixedPosition = _fixedMapInfos[mapinfoNumber].FixedPosition;
                _fixedRotation = _fixedMapInfos[mapinfoNumber].FixedRotation;
            }
            else
            {
                _mapType = MapType.None;
                _isFixedPosition = false;
            }
        }
        else
        {
            _mapType = MapType.None;
            _isFixedPosition = false;
        }

        base.Awake();
    }
}
