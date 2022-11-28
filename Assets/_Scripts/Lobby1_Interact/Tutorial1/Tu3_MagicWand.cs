using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Tu3_MagicWand : MonoBehaviourPun
{
    [Header("Ȯ���� �ش��ϴ� ���ڸ� �������� �����ּ���")]
    [SerializeField] private int[] _useMagicChance;

    private int _totalProbability = 100;

    [Header("��Ÿ���� ����ּ���")]
    [SerializeField] private Defines.CoolTime _coolTime;

    [Header("VRUI�� MagicWandPanel�� �־��ּ���")]
    [SerializeField]
    private GameObject _magicWandPanel;

    private TextMeshProUGUI _magicNameText;
    private TextMeshProUGUI _magicCoolTimeText;

    [SerializeField]
    private ParticleSystem[] _magic;
    private float _currentTime;
    private bool _checkCoolTime;

    // ����ġ�� �ʿ��� ������
    private Vector3 _wandPosition;

    private void Awake()
    {
        _magicNameText = _magicWandPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _magicCoolTimeText = _magicWandPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        _wandPosition = transform.position;

        _magicWandPanel.SetActive(false);

        _magic = new ParticleSystem[transform.childCount];

        for (int i = 0; i < transform.childCount - 1; ++i)
        {
            _magic[i] = gameObject.transform.GetChild(i).GetComponentInChildren<ParticleSystem>();
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) && !_checkCoolTime)
        {
            int RandomNumber = Random.Range(0, _totalProbability + 1);

            GetMagic(RandomNumber);

            _magicWandPanel.SetActive(true);
            _magicNameText.text = gameObject.name;

            _checkCoolTime = true;
        }

        if (_checkCoolTime)
        {
            _currentTime += Time.deltaTime;

            int _coolTimeText = (int)_coolTime;

            _coolTimeText -= (int)_currentTime;
            _magicCoolTimeText.text = _coolTimeText.ToString();

            if (_currentTime > (float)_coolTime)
            {
                _currentTime -= _currentTime;
                _checkCoolTime = false;
                _magicWandPanel.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        transform.position = _wandPosition;
        _magicWandPanel.SetActive(false);
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
