using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputTutorialTrigger : FocusableObjects
{

    public UnityEvent OnTriggered = new UnityEvent();
    
    [SerializeField] GameObject _player;
    [SerializeField] Vector3 _triggerPosition;
    [SerializeField] Quaternion _triggerRotation;


    private void Start()
    {
        OnFocus();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == _player)
        {
            if(_player.transform.rotation.y <= -70 && _player.transform.rotation.y >= -105)
            {
                OnTriggered.Invoke();
                gameObject.SetActive(false);
                transform.position = _triggerPosition;
                transform.rotation = _triggerRotation;
            }
        }
    }
}
