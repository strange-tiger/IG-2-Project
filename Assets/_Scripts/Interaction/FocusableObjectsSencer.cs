using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FocusableObjectsSencer : MonoBehaviourPun
{
    private SphereCollider _sencerCollider;
    private FocusableObjects _focusableObject;

    private bool _isTherePlayer = false;

    private void Awake()
    {
        //gameObject.layer = LayerMask.NameToLayer("PlayerSencer");\
        //gameObject.AddComponent<PhotonView>();

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
    private void OnGrabBegin()
    {
        photonView.RPC(nameof(OnGrabBeginByServer), RpcTarget.All);
    }

    [PunRPC]
    private void OnGrabBeginByServer()
    {
        Debug.Log("[FocusableObject] Grab Begin");
        gameObject.SetActive(false);
    }

    private void OnGrabEnd()
    {
        photonView.RPC(nameof(OnGrabEndByServer), RpcTarget.All);
    }

    [PunRPC]
    private void OnGrabEndByServer()
    {
        Debug.Log("[FocusableObject] Grab End");
        gameObject.SetActive(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("PlayerBody") || _isTherePlayer)
        {
            return;
        }
        Debug.Log($"[FocusableObject] 플레이어 들어옴 {other.gameObject.transform.parent.name}");

        _isTherePlayer = true;
        _focusableObject.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("PlayerBody") || !_isTherePlayer)
        {
            return;
        }
        Debug.Log($"[FocusableObject] 플레이어 나감 {other.gameObject.transform.parent.name}");

        _isTherePlayer = false;
        _focusableObject.enabled = false;
    }
}
