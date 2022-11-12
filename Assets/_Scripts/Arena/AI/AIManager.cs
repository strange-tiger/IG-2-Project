using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Angle = Defines.EDegrees;

public class AIManager : MonoBehaviour
{
    [Header("�����ڵ��� �־��ּ���")]
    [SerializeField]
    private GameObject[] _soldier;

    [Header("��ȯ�� ������ �Է� �� �ּ���")]
    [SerializeField]
    private float _size;

    private bool[] _isAlive = new bool[4];

    private Transform[] _spawnTransform = new Transform[4];
    private Transform _transform;



    private void Awake()
    {
        _transform = transform;
    }

    void Start()
    {
        for (int i = 0; i < _soldier.Length; ++i)
        {
            SetSoldierPositionAndRotation(i);

            Instantiate(_soldier[i], _spawnTransform[i].position, _spawnTransform[i].rotation);
        }
    }

    /// <summary>
    /// �ʱ� ����� ��ġ
    /// </summary>
    private void SetSoldierPositionAndRotation(int index)
    {
        _spawnTransform[index] = _transform;

        switch (index)
        {
            case 0 :
                _spawnTransform[index].position = transform.position + new Vector3(_size, 0f, 0f);
                _spawnTransform[index].rotation = Quaternion.Euler(new Vector3(0, -(int)Angle.RightAngle, 0));
                break;
            case 1:
                _spawnTransform[index].position = transform.position + new Vector3(-_size * 2, 0f, 0f);
                _spawnTransform[index].rotation = Quaternion.Euler(new Vector3(0, (int)Angle.RightAngle, 0));
                break;
            case 2:
                _spawnTransform[index].position = transform.position + new Vector3(_size, 0f, _size);
                _spawnTransform[index].rotation = Quaternion.Euler(new Vector3(0, (int)Angle.TurnAround, 0));
                break;
            case 3:
                _spawnTransform[index].position = transform.position + new Vector3(0f, 0f, -_size * 2);
                _spawnTransform[index].rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
        }

    }
}
