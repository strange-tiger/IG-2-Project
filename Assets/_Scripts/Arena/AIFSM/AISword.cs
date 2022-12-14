using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISword : MonoBehaviour
{
    [SerializeField] private Collider[] _swordHitBox;

    [Header("�⺻���� ����Ʈ�� �־��ּ���")]
    [SerializeField] private GameObject _attackEffect;

    // �ִϸ��̼� �̺�Ʈ ������ �ݶ��̴� ��
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

    // �ִϸ��̼� �̺�Ʈ ������ �ݶ��̴� ����
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
