using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObjectHealth : MonoBehaviour
{
    [SerializeField] private int _point = 1;

    // �ӽ� �Լ�
    public int Hit()
    {
        Destroy(gameObject);

        return _point;
    }
}
