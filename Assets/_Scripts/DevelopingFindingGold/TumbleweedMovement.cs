using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using UnityEngine.UI;

public class TumbleweedMovement : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 20f;
    [SerializeField] private float _getGoldTime = 3f;

    [SerializeField] private float _bounceForce = 2f;
    private Rigidbody _rigidbody;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    [SerializeField] private Transform _sliderTransform;
    [SerializeField] private Slider _slider;
    private Transform _playerTransform;
    private PlayerTumbleweedInteraction _playerInteraction;
    private bool _isTherePlayer;

    private TumbleweedSpawner _spawner;

    private readonly static Vector3 ZERO_VECTOR = Vector3.zero;
    private WaitForSeconds _waitForLifeTime;

    private void Awake()
    {
        _waitForLifeTime = new WaitForSeconds(_lifeTime);

        _rigidbody = GetComponent<Rigidbody>();

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;

        _spawner = GetComponentInParent<TumbleweedSpawner>();
    }

    private void OnEnable()
    {
        ActiveSelf();
        ResetTumbleweed();
    }

    private void ResetTumbleweed()
    {
        _rigidbody.velocity = ZERO_VECTOR;
        _rigidbody.AddForce(transform.forward * _bounceForce, ForceMode.Impulse);

        _outline.enabled = false;
        _sliderTransform.gameObject.SetActive(false);

        StartCoroutine(CoDisableSelf());
    }

    private IEnumerator CoDisableSelf()
    {
        yield return _waitForLifeTime;
        DisableSelf();
    }

    private void FixedUpdate()
    {
        if(_isTherePlayer)
        {
            _sliderTransform.rotation = Quaternion.Euler(0f, _sliderTransform.rotation.y, _sliderTransform.rotation.z);
            _sliderTransform.transform.LookAt(_playerTransform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        PlayerTumbleweedInteraction playerInteraction = other.transform.root.GetComponentInChildren<PlayerTumbleweedInteraction>();
        if(!playerInteraction || playerInteraction.IsNearTumbleweed)
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
        if(!other.CompareTag("Player"))
        {
            return;
        }

        if(_isTherePlayer && _playerTransform != other.transform.root)
        {
            return;
        }

        _outline.enabled = false;
        _isTherePlayer = false;
        _playerInteraction.IsNearTumbleweed = false;
        _sliderTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!_isTherePlayer || _playerInteraction.GrabbingTime <= 0f)
        {
            _sliderTransform.gameObject.SetActive(false);
            _slider.value = 0f;
            return;
        }

        _sliderTransform.gameObject.SetActive(true);
        _slider.value = _playerInteraction.GrabbingTime / _getGoldTime;
        
        if(_slider.value >= 1f)
        {
            StopAllCoroutines();
            _playerInteraction.GetGold(GiveRandomGold());
            DisableSelf();
        }
    }

    private int GiveRandomGold()
    {
        int randomInt = Random.Range(0, 100);
        if(randomInt < 85)
        {
            return 5;
        }
        else if(randomInt < 95)
        {
            return 30;
        }
        else
        {
            return 100;
        }
    }

    private void OnDisable()
    {
        _spawner.ReturnToTumbleweedStack(gameObject);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    private void ActiveSelf()
    {
        //gameObject.SetActive(true);
    }
}
