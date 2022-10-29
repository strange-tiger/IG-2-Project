using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EJob = Defines.EJobClass;

public class AIInfo : MonoBehaviour
{
    [SerializeField]
    private EJob _eClass;

    private int _class;

    private int _hp;
    public int HP { get { return _hp; } private set { _hp = value; } }

    private int _damage;
    public int Damage { get { return _damage; } private set { _damage = value; } }

    private void Awake()
    {
        _class = (int)_eClass;
        AISetting();
    }

    private void AISetting()
    {
        switch (_class)
        {
            case 0:
                _hp = 100;
                _damage = 10;
                break;
            case 1:
                _hp = 200;
                _damage = 20;
                break;
            case 2:
                _hp = 300;
                _damage = 30;
                break;
            case 3:
                _hp = 400;
                _damage = 40;
                break;
        }

    }
}
