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
    /// �浹�� �Ͼ�� �������� ������ ���� ��Ÿ���� �ƴϸ� �Ҹ��� ����.
    /// ��Ÿ���� 1���̴�.
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
    /// ��Ÿ���� ����Ѵ�.
    /// ���� ��Ÿ�������� _notOnCooltime ������ �Ǵ��Ѵ�.
    /// ��Ÿ���� 1���̴�.
    /// </summary>
    /// <returns></returns>
    IEnumerator Cooltime()
    {
        _notOnCooltime = false;

        yield return SOUND_COOLTIME;

        _notOnCooltime = true;
    }

    /// <summary>
    /// Ʈ���� �浹 �� �����Ѵ�.
    /// �Ű����� other�� �±װ� "Campfire"�̸� StopCountDown�� RPC�� ȣ���Ѵ�.
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
    /// Ʈ���� �浹���� ��� �� �����Ѵ�.
    /// �Ű����� other�� �±װ� "Campfire"�̸� StartCountDown�� RPC�� ȣ���Ѵ�.
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
    /// �±װ� "Campfire"�� Ʈ���� �ݶ��̴����� ����� �� �ڷ�ƾ�� ����ȴ�.
    /// Ʈ���� �ݶ��̴����� ����� �� ���� ī��Ʈ�ٿ��� �����Ͽ�, 3�ʰ� ������ �� ������Ʈ�� �����Ѵ�.
    /// �� ������Ʈ�� �����ִ� ������ ī��Ʈ�ٿ��� �����.
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
