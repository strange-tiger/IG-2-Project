using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerFocus[] _playerFocus = new PlayerFocus[2];

    [SerializeField] private OVRGazePointer _pointer;
    private OVRInputModule _eventSystemInputModule;
    private OVRRaycaster _ovrRaycaster;
    private bool _isThereUI;

    public UnityEvent InteractionOakBarrel = new UnityEvent();

    private void OnEnable()
    {
        _eventSystemInputModule = FindObjectOfType<OVRInputModule>();
        if (_eventSystemInputModule)
        {
            _eventSystemInputModule.m_Cursor = _pointer;
            _eventSystemInputModule.rayTransform = _playerFocus[0].gameObject.transform;

            _ovrRaycaster = FindObjectOfType<OVRRaycaster>();
            _ovrRaycaster.pointer = _pointer.gameObject;
            _isThereUI = true;
        }
    }

    private void Update()
    {
        if(_input.IsRay)
        {
            SettingUIInteraction();
            Interact();
        }
        else
        {
            _pointer.gameObject.SetActive(false);
        }
    }

    private void SettingUIInteraction()
    {
        if(!_isThereUI)
        {
            return;
        }

        if(_input.PrimaryController == Defines.EPrimaryController.Left)
        {
            if(_input.IsLeftRay)
            {
                SetPointerTransform(_playerFocus[0]);
            }
            else
            {
                SetPointerTransform(_playerFocus[1]);
            }
        }
        else
        {
            if (_input.IsRightRay)
            {
                SetPointerTransform(_playerFocus[1]);
            }
            else
            {
                SetPointerTransform(_playerFocus[0]);
            }
        }

        _pointer.gameObject.SetActive(true);
    }
    private void SetPointerTransform(PlayerFocus playerFocus)
    {
        _pointer.rayTransform = playerFocus.gameObject.transform;
        _eventSystemInputModule.rayTransform = playerFocus.gameObject.transform;
    }

    private void Interact()
    {
        if(!_input.InputADown)
        {
            return;
        }

        if (_input.PrimaryController == Defines.EPrimaryController.Left)
        {
            if (_input.IsLeftRay)
            {
                CheckAndInteract(_playerFocus[0]);
            }
            else
            {
                CheckAndInteract(_playerFocus[1]);
            }
        }
        else
        {
            if (_input.IsRightRay)
            {
                CheckAndInteract(_playerFocus[1]);
            }
            else
            {
                CheckAndInteract(_playerFocus[0]);
            }
        }
    }

    private void CheckAndInteract(PlayerFocus playerFocus)
    {
        if (playerFocus.HaveFocuseObject)
        {
            InteracterableObject interacterableObject = playerFocus.FocusedObject.gameObject.GetComponent<InteracterableObject>();
            if(interacterableObject)
            {
                interacterableObject.Interact();

                if (interacterableObject.gameObject.CompareTag("OakBarrel"))
                {
                    InteractionOakBarrel.Invoke();
                }
                //else if (interacterableObject.gameObject.CompareTag("Player"))
                //{
                //    MeshRenderer meshRenderer = interacterableObject.gameObject.transform.Find("OakBarrel").gameObject.GetComponent<MeshRenderer>();

                //    if (meshRenderer.enabled == true)
                //    {
                //        InteractionOakBarrel.Invoke();
                //    }
                //}
            }
        }
    }
}
