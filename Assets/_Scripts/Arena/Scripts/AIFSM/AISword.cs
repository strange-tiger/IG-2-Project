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
        if (_attackEffect != null)
        {
            _attackEffect.SetActive(true);

            for (int i = 0; i < _swordHitBox.Length; ++i)
            {
                _swordHitBox[i].enabled = true;
            }
        }

        else
        {
            for (int i = 0; i < _swordHitBox.Length; ++i)
            {
                _swordHitBox[i].enabled = true;
            }
        }
    }

    public void OffHitBox()
    {
        if (_attackEffect != null)
        {
            _attackEffect.SetActive(false);

            for (int i = 0; i < _swordHitBox.Length; ++i)
            {
                _swordHitBox[i].enabled = false;
            }
        }

        else
        {
            for (int i = 0; i < _swordHitBox.Length; ++i)
            {
                _swordHitBox[i].enabled = false;
            }
        }
    }
}
