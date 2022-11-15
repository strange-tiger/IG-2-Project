using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoldBoxInetraction : MonoBehaviour
{
    private GoldBoxSpawner _spawner;

    public UnityEvent OnGiveGold = new UnityEvent();

    private Vector3 _originalPosition;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _spawner = transform.root.GetComponentInParent<GoldBoxSpawner>();
        _rigidbody = transform.root.GetComponentInParent<Rigidbody>();
        _originalPosition = transform.position;
    }

    private void OnEnable()
    {
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(WaitForGiveGold());
    }

    private IEnumerator WaitForGiveGold()
    {
        yield return new WaitForSeconds(3f);
        OnGiveGold.Invoke();
        transform.position = _originalPosition;
        
        yield return new WaitForSeconds(3f);
        _spawner.ReturnToPoll(gameObject);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
