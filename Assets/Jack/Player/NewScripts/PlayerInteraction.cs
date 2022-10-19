using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerFocus[] _playerFocus = new PlayerFocus[2];


    private void Update()
    {
        if(_input.IsRay && _input.InputA)
        {
            if(_input.PrimaryController == Defines.EPrimaryController.Left)
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
    }

    private void CheckAndInteract(PlayerFocus playerFocus)
    {
        if (playerFocus.HaveFocuseObject)
        {
            InteracterableObject interacterableObject = playerFocus.FocusedObject.gameObject.GetComponent<InteracterableObject>();
            if(interacterableObject)
            {
                interacterableObject.Interact();
            }
        }
    }
}
