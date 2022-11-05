using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObjectHealth : MonoBehaviour
{
    [SerializeField] private int _point = 1;

    // 임시 함수
    public int Hit()
    {
        Destroy(gameObject);

        return _point;
    }
}
