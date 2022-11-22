using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputTutorialTrigger : FocusableObjects
{

    public UnityEvent OnTriggered = new UnityEvent();
    
    [SerializeField] Transform _player;
    [SerializeField] Vector3 _triggerPosition;
    [SerializeField] Quaternion _triggerRotation;
    [SerializeField] Quaternion[] _triggeredRotation;

    private void Start()
    {
        OnFocus();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(_player.localRotation.y <= _triggeredRotation[0].y && _player.localRotation.y >= -_triggeredRotation[1].y || _player.localRotation.y <= _triggeredRotation[2].y && _player.localRotation.y >= _triggeredRotation[3].y)
            {
                OnTriggered.Invoke();
                gameObject.SetActive(false);
                transform.position = _triggerPosition;
                transform.rotation = _triggerRotation;
            }
        }
    }
}
