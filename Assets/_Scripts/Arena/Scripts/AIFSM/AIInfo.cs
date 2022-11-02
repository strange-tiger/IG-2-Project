using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EJob = Defines.EJobClass;

public class AIInfo : MonoBehaviour
{
    [Header("데미지를 입력 해 주세요")]
    [SerializeField] private int _insertDamage;

    [Header("스킬 데미지를 입력 해 주세요")]
    [SerializeField] private int _insertSkill;

    private int _damage;
    public int Damage { get { return _damage; } private set { _damage = value; } }

    private int _skillDamage;
    public int SkillDamage { get { return _skillDamage; } private set { _skillDamage = value; } }

    private void Awake()
    {
        _damage = _insertDamage;
        _skillDamage = _insertSkill;
    }

}
