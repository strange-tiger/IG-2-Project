using System.Collections;
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
    /// CampfirePlace와 트리거 충돌이 일어나면 호출된다.
    /// 현재 Wood에서 실행되는 모든 코루틴을 멈춘다.
    /// </summary>
    [PunRPC]
    private void StopCountDown()
    {
        StopAllCoroutines();
        _notOnCooltime = true;
    }

    /// <summary>
    /// CampfirePlace의 트리거 콜라이더에서 벗어나면 호출된다.
    /// CoutDown 코루틴을 실행한다.
    /// </summary>
    [PunRPC]
    private void StartCountDown() => StartCoroutine(CountDown());

    /// <summary>
    /// StartCountDown()이 호출되면 실행된다.
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
