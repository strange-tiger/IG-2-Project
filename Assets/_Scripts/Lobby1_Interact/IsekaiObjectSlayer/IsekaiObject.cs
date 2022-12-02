using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IsekaiObject : MonoBehaviourPun
{
    public event Action<Vector3> ObjectSlashed;

    [SerializeField] MeshRenderer _renderer;
    [SerializeField] AudioSource _audioSource;

    private static readonly WaitForSeconds FLICK_TIME = new WaitForSeconds(0.05f);

    private bool _hitAllowed = true;

    /// <summary>
    /// 물체 소환이 완료되면 무기로 칠 수 있도록 한다.
    /// 
    /// SummonCircle에서 호출된다.
    /// </summary>
    public void ReturnHitAllowed() => _hitAllowed = true;
    
    private void OnEnable()
    {
        _hitAllowed = false;
    }

    private const string WEAPON_TAG = "IsekaiWeapon";
    private const float WEAPON_VALID_VELOCITY = 1f;
    /// <summary>
    /// 소환이 완료되었을 때, 무기와 일정 이상의 속도로 충돌하면 내부 코드를 실행한다.
    /// 
    /// 태그가 WEAPON_TAG와 일치하는 콜라이더이고 그 속도가 WEAPON_VALID_VELOCITY 이상이며 _hitAllowed가 참이면
    /// 콜라이더를 쥐고 있는 SyncOVRDistanceGrabbable의 위치를 저장하고
    /// OnHit을 호출하고 이벤트 ObjectSlashed를 통보한다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(WEAPON_TAG)
            && other.GetComponent<Rigidbody>().velocity.magnitude >= WEAPON_VALID_VELOCITY
            && _hitAllowed)
        {
            Vector3 position = other.GetComponent<SyncOVRDistanceGrabbable>().grabbedBy.transform.position;

            OnHit();

            ObjectSlashed.Invoke(position);
        }
    }

    /// <summary>
    /// 컨트롤러가 진동하고 물체가 반짝인 후 사라지게 한다.
    /// 
    /// 코루틴 Vibration을 시작하고 FlickHelper를 RPC로 호출한다.
    /// </summary>
    private void OnHit()
    {
        StartCoroutine(Vibration());

        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(FlickHelper));
        photonView.RPC(nameof(FlickHelper), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void FlickHelper() => StartCoroutine(Flick());

    /// <summary>
    /// 물체가 베이면 소리가 나며 반짝이면서 사라지게 한다.
    /// 
    /// _audioSource.PlayOneShot을 호출해 지정한 소리가 나게 한다.
    /// _hitAllowed를 거짓으로 바꾸어 더 이상의 충돌이 코드를 실행하지 않게 한다.
    /// 3번 반복해서 FLICK_TIME의 딜레이마다 오브젝트의 렌더러 _renderer의 활성화 여부를 바꾼다.
    /// 오브젝트의 위치를 Vector3.zero로 바꾸고 비활성화한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Flick()
    {
        _audioSource.PlayOneShot(_audioSource.clip);

        _hitAllowed = false;

        int count = 3;

        while (count > 0)
        {
            _renderer.enabled = false;

            yield return FLICK_TIME;

            _renderer.enabled = true;

            yield return FLICK_TIME;

            --count;
        }

        transform.localPosition = Vector3.zero;

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 무기를 잡은 컨트롤러가 진동하게 한다.
    /// 
    /// OVRInput.SetControllerVibration의 진동 시간은 2초로 고정이기에,
    /// FLICK_TIME의 딜레이 후 진동이 사라지게 한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Vibration()
    {
        OVRInput.SetControllerVibration(1f, 1f);

        yield return FLICK_TIME;

        OVRInput.SetControllerVibration(0f, 0f);
    }
}
