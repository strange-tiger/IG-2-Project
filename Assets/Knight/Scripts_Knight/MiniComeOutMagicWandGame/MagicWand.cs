using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWand : MonoBehaviour
{
    [Header("확률에 해당하는 숫자를 누적시켜 적어주세요")]
    [SerializeField] private int[] _useMagicChance;

    private int _totalProbability = 100;
    private int _probability;

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
            _probability += _useMagicChance[i];
            _magic[i] = gameObject.transform.GetChild(i).GetComponentInChildren<ParticleSystem>();
        }

        if (_probability != _totalProbability)
        {
            Debug.Log("총 확률이 100이 되지 않습니다. 확인 바랍니다.");
        }
    }

    void Update()
    {


        if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.K) && !_checkCoolTime)
        {
            int RandomNumber = Random.Range(0, _totalProbability + 1);
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
        }
    }
}
