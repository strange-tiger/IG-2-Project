using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Wood : MonoBehaviourPun
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;

    private static readonly YieldInstruction SOUND_COOLTIME = new WaitForSeconds(1f);
    private const float COUNT_DOWN_TIME = 3f;

    private SyncOVRGrabbable _grabbable;
    private bool _notOnCooltime = true;

    private void OnEnable()
    {
        _grabbable = GetComponent<SyncOVRGrabbable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_grabbable.isGrabbed && _notOnCooltime)
        {
            _audioSource.PlayOneShot(_audioClip);
            StartCoroutine(Cooltime());
        }
    }

    IEnumerator Cooltime()
    {
        _notOnCooltime = false;

        yield return SOUND_COOLTIME;

        _notOnCooltime = true;
    }

    [PunRPC]
    private void StopCountDown() => StopAllCoroutines();

    [PunRPC]
    private void StartCountDown() => StartCoroutine(CountDown());

    IEnumerator CountDown()
    {
        float countDown = 0;

        while (countDown <= COUNT_DOWN_TIME)
        {
            yield return null;
            
            if (!_grabbable.isGrabbed)
            {
                countDown += Time.deltaTime;
            }
        }

        PhotonNetwork.Destroy(gameObject);
    }
}
