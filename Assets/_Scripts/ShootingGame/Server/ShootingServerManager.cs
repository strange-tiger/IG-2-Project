using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingServerManager : LobbyChanger
{
    [SerializeField] private ShootingGameManager _shootingGameManager;
    // RoomPropertyKey는 최대한 짧아야함. Game >> g로 변경
    public const string RoomPropertyKey = "g";

    protected override void Awake()
    {
        base.Awake();

        // 플레이어에 있는 GunShoot 스크립트에  메니저를 세팅해줌
        GunShoot gun = _myPlayer.GetComponentInChildren<GunShoot>();
        gun.Init(_shootingGameManager, 
            _myPlayer.GetComponentInChildren<ShootingPlayerLoadingUI>());
    }
}
