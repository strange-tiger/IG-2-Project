using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class TumbleweedMovement : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 20f;

    [SerializeField] private float _bounceForce = 2f;
    private Rigidbody _rigidbody;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    [SerializeField] private Transform _slider;
    private Transform _player;
    private bool _isTherePlayer;

    private TumbleweedSpawner _spawner;

    private readonly static Vector3 ZERO_VECTOR = Vector3.zero;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;

        _spawner = GetComponentInParent<TumbleweedSpawner>();
    }

    private void OnEnable()
    {
        ResetTumbleweed();
    }

    private void ResetTumbleweed()
    {
        _rigidbody.velocity = ZERO_VECTOR;
        _rigidbody.AddForce(transform.forward * _bounceForce, ForceMode.Impulse);

        _outline.enabled = false;
        _slider.gameObject.SetActive(false);

        Invoke("DisableSelf", _lifeTime);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {

        if(_isTherePlayer)
        {
            _slider.rotation = Quaternion.Euler(0f, _slider.rotation.y, _slider.rotation.z);
            _slider.transform.LookAt(_player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _outline.enabled = true;
            _player = other.transform;
            _isTherePlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            _outline.enabled = false;
            _isTherePlayer = false;
            _slider.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _spawner.ReturnToTumbleweedStack(gameObject);
    }
}
