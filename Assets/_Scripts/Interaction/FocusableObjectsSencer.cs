using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusableObjectsSencer : MonoBehaviour
{
    private SphereCollider _sencerCollider;
    private FocusableObjects _focusableObject;

    private bool _isTherePlayer = false;

    private void Awake()
    {
        _sencerCollider = gameObject.AddComponent<SphereCollider>();
        _sencerCollider.isTrigger = true;
        _sencerCollider.radius = 0f;

        _focusableObject = GetComponent<FocusableObjects>();

        SetRigidbody();
    }

    private void SetRigidbody()
    {
        Rigidbody _rigidbody = GetComponent<Rigidbody>();
        if(!_rigidbody)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
        }
    }

    public void SetSencer(float sencerRadius)
    {
        _sencerCollider.radius = sencerRadius;
        _focusableObject.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("PlayerBody") || _isTherePlayer)
        {
            return;
        }
        Debug.Log("[FocusableObject] 플레이어 들어옴");

        _isTherePlayer = true;
        _focusableObject.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("PlayerBody") || !_isTherePlayer)
        {
            return;
        }
        Debug.Log("[FocusableObject] 플레이어 나감");

        _isTherePlayer = false;
        _focusableObject.enabled = false;
    }
}
