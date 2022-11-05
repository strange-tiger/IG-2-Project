using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObjectHealth : MonoBehaviour
{
    [SerializeField] private int _point = 1;

    [Header("Effects")]
    [SerializeField] private GameObject _particle;

    private AudioSource _audioSource;

    // �ӽ� �Լ�
    public int Hit()
    {
        Destroy(gameObject);

        return _point;
    }
}
