using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MagicWand : MonoBehaviourPun
{
    private enum MagicWandName
    {
        Start,
        BigMagic,
        CoinMagic,
        ElementMagic,
        EnvironmentMagic,
        FireworksMagic,
        LightMagic,
        SmokeMagic,
        End, 
    }

    [Header("확률에 해당하는 숫자를 누적시켜 적어주세요")]
    [SerializeField] private int[] _useMagicChance;

    // 0 ~ 100 사이의 랜덤값을 뽑기위한 변수
    private int _totalProbability = 100;

    [Header("쿨타임을 골라주세요")]
    [SerializeField] private Defines.CoolTime _coolTime;
    public Defines.CoolTime CoolTime { get { return _coolTime; } }

    [Header("VRUI의 MagicWandPanel을 넣어주세요")]
    [SerializeField] private GameObject _magicWandPanel;
    [SerializeField] private ParticleSystem[] _magic;

    // 마법봉 종류를 인스펙터창에서 넣어줌
    [SerializeField] private MagicWandName _magicWandName;

    // 마법봉 사용 시 뜨는 판넬에 들어갈 마법봉 이름
    private TextMeshProUGUI _magicNameText;
    // 마법봉 사용 시 뜨는 판넬에 들어갈 마법봉 쿨타임
    private TextMeshProUGUI _magicCoolTimeText;

    // 델타타임이 더해질 변수
    private float _currentTime;

    // 쿨타임이 돌고있음을 확인하는 변수
    private bool _checkCoolTime;
    public bool CheckCoolTime { get { return _checkCoolTime; } set { _checkCoolTime = value; } }

    // 쿨타임
    private int _coolTimeTextNum;
    public int CoolTimeText { get { return _coolTimeTextNum; } set { _coolTimeTextNum = value; } }

    // 원위치에 필요한 변수들
    private Vector3 _wandPosition;

    private void Awake()
    {
        // 자식 텍스트 가져오기
        _magicNameText = _magicWandPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _magicCoolTimeText = _magicWandPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        // 마법봉의 초기 위치 저장
        _wandPosition = transform.position;

        // 판넬 꺼주기
        _magicWandPanel.SetActive(false);

        // 마법 가져오기
        _magic = new ParticleSystem[transform.childCount];
        for (int i = 0; i < transform.childCount -1; ++i)
        {
            _magic[i] = gameObject.transform.GetChild(i).GetComponentInChildren<ParticleSystem>();
        }
    }

    private void OnEnable()
    {
        // 마법봉을 잡았을 때 이 스크립트가 활성화 되므로 쿨타임이 있다면 판넬을 켜서 확인시켜줌
        if (_coolTimeTextNum > 0)
        {
            _magicWandPanel.SetActive(true);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two) && !_checkCoolTime)
            {
                // 랜덤값 저장
                int RandomNumber = Random.Range(0, _totalProbability + 1);

                // 
                photonView.RPC(nameof(GetMagic), RpcTarget.All, RandomNumber);

                // 판넬 켜 주기
                _magicWandPanel.SetActive(true);

                // 마법봉의 종류에 따라 이름 변경
                if ((int)_magicWandName == 1) { _magicNameText.text = "대규모 마법"; }
                if ((int)_magicWandName == 2) { _magicNameText.text = "코인 마법"; }
                if ((int)_magicWandName == 3) { _magicNameText.text = "원소 마법"; }
                if ((int)_magicWandName == 4) { _magicNameText.text = "환경 변화 마법"; }
                if ((int)_magicWandName == 5) { _magicNameText.text = "불꽃놀이 마법"; }
                if ((int)_magicWandName == 6) { _magicNameText.text = "빛 마법"; }
                if ((int)_magicWandName == 7) { _magicNameText.text = "연기 마법"; }

                // 쿨타임이 돌고있음
                _checkCoolTime = true;
            }

            // 쿨타임 체크
            if (_checkCoolTime)
            {
                _currentTime += Time.deltaTime;
                _magicCoolTimeText.text = _coolTimeTextNum.ToString();

                if (_currentTime > (float)_coolTime)
                {
                    _currentTime -= _currentTime;
                    _magicWandPanel.SetActive(false);
                }
            }
        }
    }

    private void OnDisable()
    {
        // 스크립트가 꺼질때(그랩을 놓을 때) 초기위치로 가게 함
        transform.position = _wandPosition;
        _magicWandPanel.SetActive(false);
    }

    /// <summary>
    /// 랜덤한 int 값에 맞는 마법 사용
    /// </summary>
    /// <param name="num">랜덤 int 값</param>
    [PunRPC]
    public void GetMagic(int num)
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
