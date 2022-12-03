using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Photon.Pun;

/// <summary>
/// PlayerFocus에서 받은 포커스한 대상 중 상호작용이 가능한 오브젝트(InteracterableObject가 있는 오브젝트)를 받아 상호작용 처리를 한다.
/// 양손의 레이가 모두 어떤 대상을 포커스하고 있는 중이라면, 주 컨트롤러인 손에서 포커스 하고 있는 대상과만 상호작용을 할 수 있도록 한다.
/// 위의 처리는 UI에서도 동일하게 처리된다.
/// 추가적으로 UI에 OVRGazePointer를 띄울 수 있도록 처리해준다.
/// </summary>
public class PlayerInteraction : MonoBehaviourPun
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerFocus[] _playerFocus = new PlayerFocus[2];

    // OVR UI 인식 관련
    [SerializeField] private OVRGazePointer _pointer;
    private OVRInputModule _eventSystemInputModule;
    private OVRRaycaster _ovrRaycaster;

    private bool _isThereUI;
    public UnityEvent InteractionOakBarrel = new UnityEvent();

    private void OnEnable()
    {
        // 플레이어에게 붙어있는 EventSystem(OVRInputModule)을 갖고 옴
        _eventSystemInputModule = transform.root.GetComponentInChildren<OVRInputModule>();
        if(!_eventSystemInputModule)
        {
            // 없다면 외부에서 찾음
            _eventSystemInputModule = FindObjectOfType<OVRInputModule>();
        }

        if (_eventSystemInputModule)
        {
            // EventSystem에 OVRGazePointer 붙여주기
            _eventSystemInputModule.m_Cursor = _pointer;
            _eventSystemInputModule.rayTransform = _playerFocus[0].gameObject.transform;

            _ovrRaycaster = FindObjectOfType<OVRRaycaster>();
            _ovrRaycaster.pointer = _pointer.gameObject;
            _isThereUI = true;
        }
    }

    private void Update()
    {
        if (_input.IsRay)
        {
            SettingUIInteraction();
            Interact();
        }
        else
        {
            _pointer.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 레이로 UI와 인터렉션 할 때 주 컨트롤러에 따라 OVRGazePointer의 rayTransfrom을 변경해줌
    /// </summary>
    private void SettingUIInteraction()
    {
        if (!_isThereUI)
        {
            return;
        }

        if (_input.PrimaryController == Defines.EPrimaryController.Left)
        {
            SetPointerTransform(_input.IsLeftRay ? _playerFocus[0] : _playerFocus[1]);
        }
        else
        {
            SetPointerTransform(_input.IsRightRay ? _playerFocus[1] : _playerFocus[0]);
        }

        _pointer.gameObject.SetActive(true);
    }
    private void SetPointerTransform(PlayerFocus playerFocus)
    {
        _pointer.rayTransform = playerFocus.gameObject.transform;
        _eventSystemInputModule.rayTransform = playerFocus.gameObject.transform;
    }

    /// <summary>
    /// 포커스 하고 있는 오브젝트와의 인터렉션 실행
    /// 양손 모두 포커스 하고 있다면 주 컨트롤러가 포커스 하고 있는 대상과만 인터렉션을 진행
    /// </summary>
    private void Interact()
    {
        if (!_input.InputADown)
        {
            return;
        }

        if (_input.PrimaryController == Defines.EPrimaryController.Left)
        {
            CheckAndInteract(_input.IsLeftRay ? _playerFocus[0] : _playerFocus[1]);
        }
        else
        {
            CheckAndInteract(_input.IsRightRay ? _playerFocus[1] : _playerFocus[0]);
        }
    }

    private void CheckAndInteract(PlayerFocus playerFocus)
    {
        if (playerFocus.HaveFocuseObject)
        {
            InteracterableObject interacterableObject = playerFocus.FocusedObject.gameObject.GetComponent<InteracterableObject>();
            if (interacterableObject)
            {
                interacterableObject.Interact();
                Debug.Log($"name : {interacterableObject.name}");
                Debug.Log($"tag : {interacterableObject.tag}");
                Debug.Log($"obtag : {interacterableObject.gameObject.tag}");

                if (interacterableObject.gameObject.tag == "OakBarrel" && !photonView.IsMine)
                {
                    Debug.Log("테그가 오크통임을 확인하고 이벤트 인보크");
                    InteractionOakBarrel.Invoke();
                }
            }
        }
    }
}
