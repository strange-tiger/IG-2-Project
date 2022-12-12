using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingServerManager : LobbyChanger
{
    [SerializeField] private ShootingGameManager _shootingGameManager;
    // RoomPropertyKey�� �ִ��� ª�ƾ���. Game >> g�� ����
    public const string RoomPropertyKey = "g";

    protected override void Awake()
    {
        base.Awake();

        // �÷��̾ �ִ� GunShoot ��ũ��Ʈ��  �޴����� ��������
        GunShoot gun = _myPlayer.GetComponentInChildren<GunShoot>();
        gun.Init(_shootingGameManager, 
            _myPlayer.GetComponentInChildren<ShootingPlayerLoadingUI>());
    }
}
