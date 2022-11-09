using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootingServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private ShootingGameManager _shootingGameManager;

    private void Awake()
    {
    }
}
