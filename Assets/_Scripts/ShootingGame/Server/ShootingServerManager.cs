using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootingServerManager : LobbyChanger
{
    [SerializeField] private ShootingGameManager _shootingGameManager;

    protected override void Awake()
    {
        base.Awake();
        GunShoot gun = _myPlayer.GetComponentInChildren<GunShoot>();
        gun.SetManager(_shootingGameManager, _myPlayer.GetComponentInChildren<ShootingPlayerLoadingUI>());
    }
}
