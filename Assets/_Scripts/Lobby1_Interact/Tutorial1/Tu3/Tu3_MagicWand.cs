using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tu3_MagicWand : MonoBehaviour
{
    [Header("확률에 해당하는 숫자를 누적시켜 적어주세요")]
    [SerializeField] private int[] _useMagicChance;

    private int _totalProbability = 100;

    [Header("쿨타임을 골라주세요")]
    [SerializeField] private Defines.CoolTime _coolTime;
    public Defines.CoolTime CoolTime { get { return _coolTime; } }

    [Header("VRUI의 MagicWandPanel을 넣어주세요")]
    [SerializeField]
    private GameObject _magicWandPanel;

    private TextMeshProUGUI _magicNameText;
    private TextMeshProUGUI _magicCoolTimeText;

    [SerializeField]
    private ParticleSystem[] _magic;
    private float _currentTime;
    private bool _checkCoolTime;
    public bool CheckCoolTime { get { return _checkCoolTime; } set { _checkCoolTime = value; } }

    private int _coolTimeText;
    public int CoolTimeText { get { return _coolTimeText; } set { _coolTimeText = value; } }

    // 원위치에 필요한 변수들
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

    private void OnEnable()
    {
        if (_coolTimeText > 0)
        {
            _magicWandPanel.SetActive(true);
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) && !_checkCoolTime)
        {
            int RandomNumber = Random.Range(0, _totalProbability + 1);

            GetMagic(RandomNumber);

            _magicWandPanel.SetActive(true);
            _magicNameText.text = "불꽃놀이 마법";

            _checkCoolTime = true;
        }

        if (_checkCoolTime)
        {
            _currentTime += Time.deltaTime;
            _magicCoolTimeText.text = _coolTimeText.ToString();

            if (_currentTime > (float)_coolTime)
            {
                _currentTime -= _currentTime;
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
