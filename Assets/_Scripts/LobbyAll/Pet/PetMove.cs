using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public enum EPetMoveState
{
    IDLE,
    MOVE,
    MAX
}

public class PetMove : MonoBehaviourPun
{
    /// <summary>
    /// State가 할당 받는 경우, State가 EPetMoveState.MOVE인지 여부를 통보한다.
    /// </summary>
    public event Action<bool> OnStateChanged;
    public EPetMoveState State
    {
        get { return _state; }

        set
        {
            _state = value;
            OnStateChanged.Invoke(_state == EPetMoveState.MOVE);
        }
    }
    protected EPetMoveState _state = EPetMoveState.IDLE;

    protected int _targetId;
    protected Transform _targetTransform;
    protected NavMeshAgent _agent;
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        OnStateChanged -= ChangeMoveState;
        OnStateChanged += ChangeMoveState;

        State = EPetMoveState.IDLE;
    }

    private void OnDisable()
    {
        OnStateChanged -= ChangeMoveState;
    }

    protected const string PET_SPAWNER_TAG = "PetSpawner";
    /// <summary>
    /// 플레이어와 가까워지면 펫을 멈춘다.
    /// 
    /// 태그가 "PetSpawner"인 콜라이더와 충돌하고 콜라이더의 ViewID가 _targetId와 같으면
    /// State를 EPetMoveState.IDLE로 바꾼다(할당한다).
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PET_SPAWNER_TAG) && other.gameObject.GetPhotonView().ViewID == _targetId)
        {
            State = EPetMoveState.IDLE;
        }
    }

    /// <summary>
    /// 플레이어와 멀어지면 펫을 움직인다.
    /// 
    /// 태그가 "PetSpawner"인 콜라이더와의 충돌에서 벗어나고 콜라이더의 ViewID가 _targetId와 같으면
    /// State를 EPetMoveState.MOVE로 바꾼다(할당한다).
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PET_SPAWNER_TAG) && other.gameObject.GetPhotonView().ViewID == _targetId)
        {
            State = EPetMoveState.MOVE;
        }
    }

    /// <summary>
    /// 이 펫이 따라다닐 타겟 오브젝트와 위치를 정한다.
    /// 
    /// PetSpawner에서 펫이 생성되면서 호출된다.
    /// _targetId에 매개변수로 받은 id를 할당한다.
    /// _targetTransform에 매개변수로 받은 target을 할당한다.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="target"></param>
    public void SetTarget(int id, Transform target)
    {
        _targetId = id;
        _targetTransform = target;
    }

    protected const string PARAM_IS_WALK = "IsWalk";
    /// <summary>
    /// 펫의 애니메이터 작동을 제어하고 ChangeMoveStateHelper를 호출한다.
    /// 
    /// OnStateChanged을 구독한다.
    /// OnStateChanged가 전달하는 bool값 isMove를 받아,
    /// 펫이 갖는 애니메이터 _animator의 변수 IsWalk의 값을 결정한다.
    /// ChangeMoveStateHelper를 호출한다.
    /// </summary>
    /// <param name="isMove"></param>
    private void ChangeMoveState(bool isMove)
    {
        _animator.SetBool(PARAM_IS_WALK, isMove);

        ChangeMoveStateHelper(isMove);
    }

    /// <summary>
    /// 펫이 움직일 지 말지를 결정한다.
    /// 
    /// 매개변수로 받는 isMove가 참이면 TrackDestination 코루틴을 실행한다.
    /// 펫의 네비메쉬 에이전트 _agent가 멈출지 말지를 결정한다.
    /// </summary>
    /// <param name="isMove"></param>
    protected void ChangeMoveStateHelper(bool isMove)
    {
        if (isMove)
        {
            StartCoroutine(TrackDestination());
        }

        _agent.isStopped = !isMove;
    }

    /// <summary>
    /// 플레이어의 위치를 목적지로 지정한다.
    /// 
    /// _state가 EPetMoveState.MOVE인 동안, _agent.SetDestination을 호출하여
    /// _agent의 목적지 (Vector3)destination에 _targetTransform.position을 할당한다.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator TrackDestination()
    {
        while(_state == EPetMoveState.MOVE)
        {
            _agent.SetDestination(_targetTransform.position);
            yield return null;
        }
    }
}