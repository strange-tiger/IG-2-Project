using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Tumbleweed : MonoBehaviourPun
{
    private enum ECoinGrade
    {
        Common,
        Rare,
        Epic,
        Max,
    }

    [Header("기본 스팩")]
    [SerializeField] private float _lifeTime = 20f;
    [SerializeField] private float _grabToGetGoldTime = 3f;
    [SerializeField] private float _disableAfterGetGoldOffsetTime = 1f;
    [Header("골드 스팩")]
    [SerializeField] private int[] _goldCoinGiveCount = new int[(int)ECoinGrade.Max];
    [SerializeField] private float[] _goldCoinRate = new float[(int)ECoinGrade.Max];
    private float _maxGoldCoinRate;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    [Header("이동 관련")]
    [SerializeField] private float _bounceForce = 2f;
    private Rigidbody _rigidbody;

    [Header("UI")]
    [SerializeField] private Transform _UITransform;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _getGoldPanel;
    [SerializeField] private TextMeshProUGUI _goldCountText;

    [Header("사운드")]
    [SerializeField] private AudioClip[] _goldCoinAudioClips = new AudioClip[(int)ECoinGrade.Max];
    private AudioSource _audioSource;

    // 플레이어 인식 관련
    private Transform _playerTransform;
    private PlayerTumbleweedInteraction _playerInteraction;
    private bool _isTherePlayer;
    private bool _isGetCoin;

    // 스포너
    private TumbleweedSpawner _spawner;

    // 사용 예정 변수
    private readonly static Vector3 _ZERO_VECTOR = Vector3.zero;
    private WaitForSeconds _waitForLifeTime;
    private WaitForSeconds _waitForDisable;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        //if(photonView.IsMine)
        {
            // 자주 사용하는 WaitForSeconds 생성
            _waitForLifeTime = new WaitForSeconds(_lifeTime);
            _waitForDisable = new WaitForSeconds(_disableAfterGetGoldOffsetTime);

            _rigidbody = GetComponent<Rigidbody>();

            _outline = GetComponent<Outlinable>();
            _outline.AddAllChildRenderersToRenderingList();
            _outline.OutlineParameters.Color = _outlineColor;

            _spawner = GetComponentInParent<TumbleweedSpawner>();

            _audioSource = GetComponent<AudioSource>();

            _meshRenderer = GetComponent<MeshRenderer>();

            // 확률 계산을 위한 총 확률 구하기
            _maxGoldCoinRate = 0f;
            foreach (float rate in _goldCoinRate)
            {
                _maxGoldCoinRate += rate;
            }
        }
    }

    private void OnEnable()
    {
        //if(photonView.IsMine)
        {
            ResetTumbleweed();
        }
    }

    // 회전초 초기화
    private void ResetTumbleweed()
    {
        // 물리 초기화 후 다시 던지기
        _rigidbody.velocity = _ZERO_VECTOR;
        _rigidbody.AddForce(transform.forward * _bounceForce, ForceMode.Impulse);

        _outline.enabled = false;
        
        _meshRenderer.enabled = true;

        // UI 초기화
        _slider.gameObject.SetActive(false);
        _slider.value = 0f;
        _getGoldPanel.SetActive(false);

        // 조건 초기화
        _isTherePlayer = false;
        _isGetCoin = false;

        StopAllCoroutines();
        StartCoroutine(CoDisableSelf());
    }

    // 일정 수명 후 자기 자신을 Disable 함
    private IEnumerator CoDisableSelf()
    {
        yield return _waitForLifeTime;
        photonView.RPC("DisableSelf", RpcTarget.All);
        //DisableSelf();
    }

    private void FixedUpdate()
    {
        if(_isTherePlayer)
        {
            // UI 위치 고정
            _UITransform.rotation = Quaternion.Euler(0f, _UITransform.rotation.y, _UITransform.rotation.z);
            _UITransform.transform.LookAt(_playerTransform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GetPlayer(other, "TriggerEnter");
    }

    private void OnTriggerStay(Collider other)
    {
        GetPlayer(other, "TriggerStay");
    }

    private void GetPlayer(Collider other, string debugMessage)
    {
        if(_isTherePlayer)
        {
            return;
        }

        if (!other.CompareTag("PlayerBody"))
        {
            return;
        }

        PlayerTumbleweedInteraction playerInteraction =
            other.transform.root.GetComponentInChildren<PlayerTumbleweedInteraction>();
        if (!playerInteraction || playerInteraction.IsNearTumbleweed)
        {
            return;
        }

        _outline.enabled = true;

        _playerTransform = other.transform.root;

        _playerInteraction = playerInteraction;
        _playerInteraction.IsNearTumbleweed = true;

        _isTherePlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("PlayerBody") || !_isTherePlayer)
        {
            return;
        }

        if(_isTherePlayer && _playerTransform != other.transform.root)
        {
            return;
        }

        _outline.enabled = false;
        
        _playerInteraction.IsNearTumbleweed = false;
        
        _slider.gameObject.SetActive(false);
        
        _isTherePlayer = false;
    }

    private void Update()
    {
        if(_isGetCoin)
        {
            return;
        }

        if(!_isTherePlayer || _playerInteraction.GrabbingTime <= 0f)
        {
            _slider.value = 0f;
            _slider.gameObject.SetActive(false);

            return;
        }

        _slider.value = _playerInteraction.GrabbingTime / _grabToGetGoldTime;
        _slider.gameObject.SetActive(true);
        
        if(_slider.value >= 1f)
        {
            _isGetCoin = true;

            // 성공하면 재자리에 멈추기
            _rigidbody.velocity = _ZERO_VECTOR;

            StopAllCoroutines();
            
            // 골드 전달
            _playerInteraction.GetGold(GiveRandomGold());
            
            _meshRenderer.enabled = false;

            StartCoroutine(DisableSelfAfterGetGold());
            //Invoke("DisableSelf", 1f);
        }
    }

    private int GiveRandomGold()
    {
        float randomInt = Random.Range(0f, _maxGoldCoinRate);

        float coinRate = 0f;
        for(int i = 0; i < (int) ECoinGrade.Max; ++i)
        {
            coinRate += _goldCoinRate[i];
            if(randomInt < coinRate)
            {
                return GiveCoinEffect(i);
            }
        }

        return -1;
    }
    private int GiveCoinEffect(int grade)
    {
        _audioSource.PlayOneShot(_goldCoinAudioClips[grade]);

        _slider.gameObject.SetActive(false);
        _goldCountText.text = "+" + _goldCoinGiveCount[grade];
        _getGoldPanel.SetActive(true);

        return _goldCoinGiveCount[grade];
    }

    private void OnDisable()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        _spawner.ReturnToTumbleweedStack(gameObject);
    }

    [PunRPC]
    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DisableSelfAfterGetGold()
    {
        yield return _waitForDisable;
        photonView.RPC("DisableSelf", RpcTarget.All);
    }

    [PunRPC]
    public void ActiveSelf()
    {
        gameObject.SetActive(true);
    }
}
