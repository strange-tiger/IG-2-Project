using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class TumbleweedMovement : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 2f;
    private Rigidbody _rigidbody;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    [SerializeField] private Transform _slider;
    private Transform _player;
    private bool _isTherePlayer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * _bounceForce, ForceMode.Impulse);

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;
        _outline.enabled = false;

        Destroy(gameObject, 20f);
    }

    private void FixedUpdate()
    {
        _slider.rotation = Quaternion.Euler(0f, _slider.rotation.y, _slider.rotation.z);

        if(_isTherePlayer)
        {
            _slider.transform.LookAt(_player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
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
}
