using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPOOutline;
using OnQuestEnd = QuestConducter.QuestEnd;

public class TumbleweedTutorial : MonoBehaviour
{
    public event OnQuestEnd OnQuestEnd;

    [SerializeField] private float _grabToGetGoldTime = 3f;
    [SerializeField] private float _disableAfterGetGoldOffsetTime = 1f;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    [Header("Movement")]
    [SerializeField] private float _bounceForce = 2f;
    private Rigidbody _rigidbody;

    [Header("UI")]
    [SerializeField] private Transform _UITransform;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _getGoldPanel;
    [SerializeField] private TextMeshProUGUI _goldCountText;

    private AudioSource _audioSource;

    // 플레이어 인식 관련
    private Transform _playerTransform;
    private PlayerTumbleweedInteraction _playerInteraction;
    private bool _isTherePlayer;
    private bool _isGetCoin;

    // 사용 예정 변수
    private readonly static Vector3 _ZERO_VECTOR = Vector3.zero;
    private WaitForSeconds _waitForDisable;

    private MeshRenderer _meshRenderer;

    private Vector3 _originalPosition;

    private void Awake()
    {
        _waitForDisable = new WaitForSeconds(_disableAfterGetGoldOffsetTime);

        _rigidbody = GetComponent<Rigidbody>();

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;

        _audioSource = GetComponent<AudioSource>();

        _meshRenderer = GetComponent<MeshRenderer>();

        _originalPosition = transform.position;

        OnQuestEnd -= GiveCoinEffect;
        OnQuestEnd += GiveCoinEffect;
    }

    private void OnEnable()
    {
        ResetTumbleweed();
    }

    // 회전초 초기화
    private void ResetTumbleweed()
    {
        transform.position = _originalPosition;

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
    }

    private void FixedUpdate()
    {
        if (_isTherePlayer)
        {
            // UI 위치 고정
            _UITransform.rotation = Quaternion.Euler(0f, _UITransform.rotation.y, _UITransform.rotation.z);
            _UITransform.transform.LookAt(_playerTransform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerBody"))
        {
            return;
        }

        PlayerTumbleweedInteraction playerInteraction =
            other.transform.root.GetComponentInChildren<PlayerTumbleweedInteraction>();
        if (!playerInteraction)
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
        if (!other.CompareTag("PlayerBody") || !_isTherePlayer)
        {
            return;
        }

        if (_isTherePlayer && _playerTransform != other.transform.root)
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
        if (_isGetCoin)
        {
            return;
        }

        if (!_isTherePlayer || _playerInteraction.InteractingTime <= 0f)
        {
            _slider.value = 0f;
            _slider.gameObject.SetActive(false);

            return;
        }

        _slider.value = _playerInteraction.InteractingTime / _grabToGetGoldTime;
        _slider.gameObject.SetActive(true);

        // 게이지가 모두 참, 퀘스트 종료
        if (_slider.value >= 1f)
        {
            _isGetCoin = true;
            OnQuestEnd.Invoke();
        }
    }

    private void GiveCoinEffect()
    {
        _audioSource.Play();

        _slider.gameObject.SetActive(false);
        _goldCountText.text = "+1";
        _getGoldPanel.SetActive(true);
        _rigidbody.velocity = _ZERO_VECTOR;

        _meshRenderer.enabled = false;

        StopAllCoroutines();
        StartCoroutine(DisableSelfAfterGetGold());
}

    private IEnumerator DisableSelfAfterGetGold()
    {
        yield return _waitForDisable;
        gameObject.SetActive(false);
    }
}
