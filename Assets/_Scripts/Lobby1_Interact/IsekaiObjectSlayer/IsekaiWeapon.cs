using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using _IRM = Defines.RPC.IsekaiRPCMethodName;

public class IsekaiWeapon : MonoBehaviourPun
{
    [SerializeField] Collider[] _attackPoints;
    
    private static readonly WaitForSeconds RETURN_DELAY = new WaitForSeconds(0.5f);
    
    private SyncOVRDistanceGrabbable _grabbable;
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;
    private Vector3 _initPosition;
    private Vector3 _initRotation;
    private bool _isUsing = false;

    private void Awake()
    {
        _grabbable = GetComponent<SyncOVRDistanceGrabbable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _initPosition = transform.position;
        _initRotation = transform.rotation.eulerAngles;

        ChangeSetting(false);
    }

    private void Update()
    {
        if (_grabbable.isGrabbed && !_isUsing)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(MonitorWeaponGrabbed());
        }
    }

    private IEnumerator MonitorWeaponGrabbed()
    {
        ChangeSetting(_grabbable.isGrabbed);

        while (_grabbable.isGrabbed)
        {
            yield return null;
        }

        _rigidbody.useGravity = true;

        yield return RETURN_DELAY;

        _rigidbody.useGravity = false;

        ChangeSetting(_grabbable.isGrabbed);

        transform.position = _initPosition;
        transform.rotation = Quaternion.Euler(_initRotation);

        photonView.RPC(_IRM.ReturnWeapon, RpcTarget.All);
    }

    private void ChangeSetting(bool isGrabbed)
    {
        _isUsing = isGrabbed;

        foreach (Collider attackPoint in _attackPoints)
        {
            attackPoint.enabled = isGrabbed;
        }
    }

    [PunRPC]
    private void ReturnWeapon() => gameObject.SetActive(false);
}
