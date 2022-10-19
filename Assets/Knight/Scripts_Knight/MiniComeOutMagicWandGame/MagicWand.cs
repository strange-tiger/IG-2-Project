using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    [Header("순서에 맞게 확률을 적어주세요")]
    [SerializeField] private int[] _useMagicChance;

    [Header("쿨타임을 골라주세요")]
    [SerializeField] private Defines.CoolTime _coolTime;

    private ParticleSystem[] _magic;
    private float _currentTime;
    private bool _checkCoolTime;

    void Start()
    {
        _magic = new ParticleSystem[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            _magic[i] = gameObject.transform.GetChild(i).GetComponentInChildren<ParticleSystem>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !_checkCoolTime)
        {
            int RandomNumber = Random.Range(0, 101);
            Debug.Log(RandomNumber);
            GetMagic(RandomNumber);

            _checkCoolTime = true;
        }

        if (_checkCoolTime)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > (float)_coolTime)
            {
                _currentTime -= _currentTime;
                _checkCoolTime = false;
            }
        }
    }

    private void GetMagic(int num)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (num < _useMagicChance[i])
            {
                _magic[i].gameObject.SetActive(true);
                break;
            }
            else if (num < _useMagicChance[i])
            {
                _magic[i].gameObject.SetActive(true);
                break;
            }
            else if (num < _useMagicChance[i])
            {
                _magic[i].gameObject.SetActive(true);
                break;
            }
        }
    }
}
