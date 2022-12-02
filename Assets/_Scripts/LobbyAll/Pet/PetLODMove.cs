using UnityEngine;
using UnityEngine.AI;

public class PetLODMove : PetMove
{
    private Animator[] _animators;

    private void Awake()
    {
        _animators = new Animator[transform.childCount];
        _animators = GetComponentsInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        OnStateChanged -= ChangeMoveState;
        OnStateChanged += ChangeMoveState;
    }

    private void OnDisable()
    {
        OnStateChanged -= ChangeMoveState;
    }

    /// <summary>
    /// LOD Group�� ���� ���� �ִϸ����Ͱ� ���� �� �־�, �� ���¸� �� ���� �����ϱ� ����
    /// ���� PetMove�� ChangeMoveState�ʹ� �ٸ� ������ �ʿ��ϴ�.
    /// _animators�� ���� ��� �ִϸ����͸� �迭�� �����Ѵ�.
    /// OnStateChanged�� �뺸�ϸ� ȣ��ȴ�.
    /// �Ű������� �޴� bool�� isMove�� _animators�� �ִϸ������� ���� "IsWalk"�� �Ҵ��Ѵ�.
    /// ChangeMoveStateHelper�� ȣ���Ѵ�.
    /// </summary>
    /// <param name="isMove"></param>
    private void ChangeMoveState(bool isMove)
    {
        foreach (Animator animator in _animators)
        {
            animator.SetBool(PARAM_IS_WALK, isMove);
        }

        ChangeMoveStateHelper(isMove);
    }
}
