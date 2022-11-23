using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ShootingServerManager : LobbyChanger
{
    [SerializeField] private ShootingGameManager _shootingGameManager;
    // RoomPropertyKey�� �ִ��� ª�ƾ���. Game >> g�� ����
    public const string RoomPropertyKey = "g";

    protected override void Awake()
    {
        base.Awake();
        GunShoot gun = _myPlayer.GetComponentInChildren<GunShoot>();
        gun.SetManager(_shootingGameManager, 
            _myPlayer.GetComponentInChildren<ShootingPlayerLoadingUI>());
    }
}
