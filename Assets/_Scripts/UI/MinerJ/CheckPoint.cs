using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("�ּҷ� ��� üũ����Ʈ ����")]
    [SerializeField] private int _minAngle = 90;
    private int _maxAngle = 360;

    private int _angle;
    public int Angle { get { return _angle; } set { _angle = value; } }

    public void RandNum()
    {
        Angle = Random.Range(_minAngle, _maxAngle);
        transform.localRotation = Quaternion.Euler(0, 180, Angle);
    }
}
