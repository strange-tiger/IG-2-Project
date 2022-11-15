using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class CustomizeNPC : InteracterableObject
{

    [SerializeField] GameObject _customizeNPCMenu;


    public override void Interact()
    {
        base.Interact();



        _customizeNPCMenu.SetActive(true);
    }

 
}
