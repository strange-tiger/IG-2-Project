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
    /// LOD Group을 갖는 펫은 애니메이터가 여러 개 있어, 그 상태를 한 번에 관리하기 위해
    /// 기존 PetMove의 ChangeMoveState와는 다른 로직이 필요하다.
    /// _animators로 펫의 모든 애니메이터를 배열로 관리한다.
    /// OnStateChanged가 통보하면 호출된다.
    /// 매개변수로 받는 bool값 isMove을 _animators의 애니메이터의 변수 "IsWalk"에 할당한다.
    /// ChangeMoveStateHelper를 호출한다.
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
