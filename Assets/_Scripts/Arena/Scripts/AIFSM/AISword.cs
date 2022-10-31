using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISword : MonoBehaviour
{
    [SerializeField] private Collider[] _swordHitBox;

    [Header("기본공격 이펙트를 넣어주세요")]
    [SerializeField] private GameObject _attackEffect;

    public void OnHitBox()
    {
        for (int i = 0; i < _swordHitBox.Length; ++i)
        {
            _swordHitBox[i].enabled = true;
        }

        if (_attackEffect != null)
        {
            _attackEffect.SetActive(true);
        }
        else
        {
            return;
        }
    }

    public void OffHitBox()
    {
        for (int i = 0; i < _swordHitBox.Length; ++i)
        {
            _swordHitBox[i].enabled = false;
        }

        if (_attackEffect != null)
        {
            _attackEffect.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
