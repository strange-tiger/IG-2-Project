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
    private const string CAMPFIRE_TAG = "Campfire";

    private Coroutine _countDown;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
            photonView.RPC(STOP_COUNTDOWN, RpcTarget.All);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
            photonView.RPC(START_COUNTDOWN, RpcTarget.All);
    }

    private const string STOP_COUNTDOWN = "StopCountDown";
    [PunRPC]
    private void StopCountDown()
    {
        if (_countDown != null) StopCoroutine(_countDown);
    }

    private const string START_COUNTDOWN = "StartCountDown";
    [PunRPC]
    private void StartCountDown() => _countDown = StartCoroutine(CountDown());

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
