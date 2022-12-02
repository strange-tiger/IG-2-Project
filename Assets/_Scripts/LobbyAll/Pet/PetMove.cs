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
    /// State�� �Ҵ� �޴� ���, State�� EPetMoveState.MOVE���� ���θ� �뺸�Ѵ�.
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
    /// �÷��̾�� ��������� ���� �����.
    /// 
    /// �±װ� "PetSpawner"�� �ݶ��̴��� �浹�ϰ� �ݶ��̴��� ViewID�� _targetId�� ������
    /// State�� EPetMoveState.IDLE�� �ٲ۴�(�Ҵ��Ѵ�).
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
    /// �÷��̾�� �־����� ���� �����δ�.
    /// 
    /// �±װ� "PetSpawner"�� �ݶ��̴����� �浹���� ����� �ݶ��̴��� ViewID�� _targetId�� ������
    /// State�� EPetMoveState.MOVE�� �ٲ۴�(�Ҵ��Ѵ�).
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
    /// �� ���� ����ٴ� Ÿ�� ������Ʈ�� ��ġ�� ���Ѵ�.
    /// 
    /// PetSpawner���� ���� �����Ǹ鼭 ȣ��ȴ�.
    /// _targetId�� �Ű������� ���� id�� �Ҵ��Ѵ�.
    /// _targetTransform�� �Ű������� ���� target�� �Ҵ��Ѵ�.
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
    /// ���� �ִϸ����� �۵��� �����ϰ� ChangeMoveStateHelper�� ȣ���Ѵ�.
    /// 
    /// OnStateChanged�� �����Ѵ�.
    /// OnStateChanged�� �����ϴ� bool�� isMove�� �޾�,
    /// ���� ���� �ִϸ����� _animator�� ���� IsWalk�� ���� �����Ѵ�.
    /// ChangeMoveStateHelper�� ȣ���Ѵ�.
    /// </summary>
    /// <param name="isMove"></param>
    private void ChangeMoveState(bool isMove)
    {
        _animator.SetBool(PARAM_IS_WALK, isMove);

        ChangeMoveStateHelper(isMove);
    }

    /// <summary>
    /// ���� ������ �� ������ �����Ѵ�.
    /// 
    /// �Ű������� �޴� isMove�� ���̸� TrackDestination �ڷ�ƾ�� �����Ѵ�.
    /// ���� �׺�޽� ������Ʈ _agent�� ������ ������ �����Ѵ�.
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
    /// �÷��̾��� ��ġ�� �������� �����Ѵ�.
    /// 
    /// _state�� EPetMoveState.MOVE�� ����, _agent.SetDestination�� ȣ���Ͽ�
    /// _agent�� ������ (Vector3)destination�� _targetTransform.position�� �Ҵ��Ѵ�.
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