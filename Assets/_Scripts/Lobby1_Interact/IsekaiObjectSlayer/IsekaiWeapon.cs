using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsekaiWeapon : MonoBehaviour
{
    [SerializeField] Collider[] _attackPoints;
    
    private static readonly WaitForSeconds RETURN_DELAY = new WaitForSeconds(0.5f);
    
    private SyncOVRGrabbable _grabbable;
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;
    private Vector3 _initPosition;
    private bool _isUsing = false;

    private void Awake()
    {
        _grabbable = GetComponent<SyncOVRGrabbable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _initPosition = transform.position;

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

        yield return RETURN_DELAY;

        ChangeSetting(_grabbable.isGrabbed);

        transform.position = _initPosition;
        gameObject.SetActive(false);
    }

    private void ChangeSetting(bool isGrabbed)
    {
        foreach (Collider attackPoint in _attackPoints)
        {
            attackPoint.enabled = isGrabbed;
        }
        _rigidbody.useGravity = isGrabbed;
        _rigidbody.isKinematic = !isGrabbed;
    }


}
