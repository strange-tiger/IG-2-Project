using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitUI : MonoBehaviour
{
    private GunShoot _gun;
    [SerializeField] private float _hitUIDisableTime = 0.5f;
    private WaitForSeconds _waitForDisable;

    private void Awake()
    {
        _gun = transform.root.GetComponentInChildren<GunShoot>();
        _waitForDisable = new WaitForSeconds(_hitUIDisableTime);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(CoDisableSelf());
    }

    private IEnumerator CoDisableSelf()
    {
        yield return _waitForDisable;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _gun.ReturnToHitUIPull(gameObject);
    }
}
