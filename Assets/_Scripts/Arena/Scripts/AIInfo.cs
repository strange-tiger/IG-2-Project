using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EJob = Defines.EJobClass;

public class AIInfo : MonoBehaviour
{
    [Header("데미지를 입력 해 주세요")]
    [SerializeField] private int _insertDamage;


    private int _damage;
    public int Damage { get { return _damage; } private set { _damage = value; } }

    private void Awake()
    {
        _damage = _insertDamage;
    }

}
