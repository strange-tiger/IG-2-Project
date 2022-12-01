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

    private SyncOVRGrabbable _grabbable;
    private bool _notOnCooltime = true;

    private void OnEnable()
    {
        _grabbable = GetComponent<SyncOVRGrabbable>();
    }

    /// <summary>
    /// 충돌이 일어나고 잡혀있지 않으며 현재 쿨타임이 아니면 소리를 낸다.
    /// 쿨타임은 1초이다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (_grabbable.isGrabbed && _notOnCooltime)
        {
            _audioSource.PlayOneShot(_audioClip);
            StartCoroutine(Cooltime());
        }
    }

    /// <summary>
    /// 쿨타임을 계산한다.
    /// 현재 쿨타임인지를 _notOnCooltime 변수로 판단한다.
    /// 쿨타임은 1초이다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Cooltime()
    {
        _notOnCooltime = false;

        yield return SOUND_COOLTIME;

        _notOnCooltime = true;
    }

    /// <summary>
    /// 트리거 충돌 시 실행한다.
    /// 매개변수 other의 태그가 "Campfire"이면 StopCountDown을 RPC로 호출한다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
        {
            photonView.RPC(STOP_COUNTDOWN, RpcTarget.All);
        }
    }

    /// <summary>
    /// 트리거 충돌에서 벗어날 시 실행한다.
    /// 매개변수 other의 태그가 "Campfire"이면 StartCountDown을 RPC로 호출한다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
        {
            photonView.RPC(START_COUNTDOWN, RpcTarget.All);
        }
    }

    private const string STOP_COUNTDOWN = "StopCountDown";
    [PunRPC]
    private void StopCountDown() => StopAllCoroutines();

    private const string START_COUNTDOWN = "StartCountDown";
    [PunRPC]
    private void StartCountDown() => StartCoroutine(CountDown());

    /// <summary>
    /// 태그가 "Campfire"인 트리거 콜라이더에서 벗어나면 이 코루틴이 실행된다.
    /// 트리거 콜라이더에서 벗어났을 때 부터 카운트다운을 시작하여, 3초가 지나면 이 오브젝트를 삭제한다.
    /// 이 오브젝트가 잡혀있는 동안은 카운트다운을 멈춘다.
    /// </summary>
    /// <returns></returns>
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
