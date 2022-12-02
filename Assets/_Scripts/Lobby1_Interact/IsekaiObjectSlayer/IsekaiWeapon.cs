using System.Collections;
using UnityEngine;
using Photon.Pun;

public class IsekaiWeapon : MonoBehaviourPun
{
    [SerializeField] Collider[] _attackPoints;

    [SerializeField] Rigidbody _velocityChecker;

    private static readonly WaitForSeconds RETURN_DELAY = new WaitForSeconds(0.5f);
    
    public float Velocity { get; private set; }

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

    /// <summary>
    /// 무기가 잡혀있고 사용 중이 아니라면 다시 무기의 상태를 점검한다.
    /// 
    /// 무기가 잡혀있는 동안(_grabbable.isGrabbed == true) 무기의 공격부(_attackPoints)를 활성화하기 위해 MonitorWeaponCoroutine을 호출한다.
    /// </summary>
    private void Update()
    {
        if (_grabbable.isGrabbed && !_isUsing)
        {
            MonitorWeaponCoroutine();
        }
    }

    private void MonitorWeaponVelocity()
    {
        _velocityChecker.MovePosition(transform.position);
        Velocity = _velocityChecker.velocity.magnitude;
    }

    /// <summary>
    /// 현재 작동하고 있는 코루틴 _coroutine이 있다면 멈춘다.
    /// 코루틴 MonitorWeaponGrabbed를 시작하고 _coroutine에 저장한다.
    /// </summary>
    private void MonitorWeaponCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(MonitorWeaponGrabbed());
    }

    /// <summary>
    /// 무기를 잡고 있는 동안 공격부를 활성화하고, 잡혀있지 않다면 공격부를 비활성화한다.
    /// 무기를 놓치면 일정 시간 후 무기 보관대로 무기를 돌려보낸다.
    /// 
    /// ChangeSetting을 호출해 isGrabbed에 따라 _attackPoints 요소들을 활성화한다.
    /// 놓치면 다시 ChangeSetting을 호출해 _attackPoints 요소들을 비활성화한다.
    /// RETURN_DELAY의 딜레이 동안 MonitorWeaponCoroutine이 다시 호출되지 않으면 ReturnWeapon을 호출한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MonitorWeaponGrabbed()
    {
        ChangeSetting(_grabbable.isGrabbed);

        while (_grabbable.isGrabbed)
        {
            MonitorWeaponVelocity();
            yield return null;
        }

        ChangeSetting(false);

        yield return RETURN_DELAY;

        ReturnWeapon();
    }

    /// <summary>
    /// 잡혀있는지에 따라 공격부 활성화, 사용 중 여부, 무게 사용을 변경한다.
    /// </summary>
    /// <param name="isGrabbed"></param>
    private void ChangeSetting(bool isGrabbed)
    {
        _isUsing = isGrabbed;

        _rigidbody.useGravity = !isGrabbed;

        foreach (Collider attackPoint in _attackPoints)
        {
            attackPoint.enabled = isGrabbed;
        }
    }

    /// <summary>
    /// 무기를 무기 보관대로 되돌린다.
    /// 
    /// 무기의 위치를 초기 위치 _initPosition으로 변경한다.
    /// DeactivateWeapon을 RPC로 호출한다.
    /// </summary>
    private void ReturnWeapon()
    {
        transform.position = _initPosition;
        transform.rotation = Quaternion.Euler(_initRotation);

        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(DeactivateWeapon));
        photonView.RPC(nameof(DeactivateWeapon), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void DeactivateWeapon() => gameObject.SetActive(false);
}
