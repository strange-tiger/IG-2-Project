using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 특정 행동을 해야 튜토리얼을 넘어가게 해주는 발판 트리거
public class InputTutorialTrigger : FocusableObjects
{
    public UnityEvent OnTriggered = new UnityEvent();

    [Header("Trigger Transform")]
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _triggerPosition;
    [SerializeField] private Quaternion _triggerRotation;
    [SerializeField] private Quaternion[] _triggeredRotation;

    // 튜토리얼 발판의 아웃라인을 출력시킴
    private void Start()
    {
        OnFocus();
    }

    // 플레이어가 발판 위치에 들어가면 Rotation을 확인하여 OnTriggered 이벤트를 호출함.
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player.localRotation.y <= _triggeredRotation[0].y && _player.localRotation.y >= -_triggeredRotation[1].y || _player.localRotation.y <= _triggeredRotation[2].y && _player.localRotation.y >= _triggeredRotation[3].y)
            {
                OnTriggered.Invoke();
                gameObject.SetActive(false);
                transform.position = _triggerPosition;
                transform.rotation = _triggerRotation;
            }
        }
    }
}
