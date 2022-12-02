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
        gameObject.layer = LayerMask.NameToLayer("PlayerSencer");

        _sencerCollider = gameObject.AddComponent<SphereCollider>();
        _sencerCollider.isTrigger = true;
        _sencerCollider.radius = 0f;

        SetRigidbody();
    }

    private void SetRigidbody()
    {
        Rigidbody _rigidbody = GetComponent<Rigidbody>();
        if(!_rigidbody)
        {
            _rigidbody = GetComponentInParent<Rigidbody>();
            if(!_rigidbody)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.useGravity = false;
                _rigidbody.angularDrag = 1000;
                _rigidbody.mass = 1000;
                _rigidbody.drag = 1000;
            }
        }
    }

    public void SetSencer(float sencerRadius, FocusableObjects focusableObject)
    {
        _sencerCollider.radius = sencerRadius;
        _focusableObject = focusableObject;
        _focusableObject.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("PlayerBody") || _isTherePlayer)
        {
            return;
        }
        Debug.Log($"[FocusableObject] �÷��̾� ���� {other.gameObject.transform.parent.name}");

        _isTherePlayer = true;
        _focusableObject.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("PlayerBody") || !_isTherePlayer)
        {
            return;
        }
        Debug.Log($"[FocusableObject] �÷��̾� ���� {other.gameObject.transform.parent.name}");

        _isTherePlayer = false;
        _focusableObject.enabled = false;
    }
}
