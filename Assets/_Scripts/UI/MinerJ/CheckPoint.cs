using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private int _angle;
    [SerializeField]
    private int _minAngle = 90; // 최소 값 ( - fillAmount 값 제외)
    private int _maxAngle = 360;
    public int Angle { get { return _angle; } set { _angle = value; } }

    public void RandNum()
    {
        Angle = Random.Range(_minAngle, _maxAngle);
        transform.localRotation = Quaternion.Euler(0, 180, Angle);
        //transform.Rotate(0, 180, Angle);
    }
}
