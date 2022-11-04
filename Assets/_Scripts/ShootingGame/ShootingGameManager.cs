using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGameManager : MonoBehaviour
{
    public float PlayTime { get; set; }

    private const int _MAX_PLAYER_COUNT = 4;
    [SerializeField] private Color[] _playerColors = {
        new Color(255/255, 50/255, 50/255),
        new Color(50/255, 140/255, 255/255),
        new Color(255/255, 50/255, 255/255),
        new Color(53/255, 227/255, 34/255)
    };
}
