using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class CustomizeNPC : InteracterableObject
{
    [SerializeField] private GameObject _customizeNPCMenu;

    // 커스터마이징 NPC에게 상호작용하면 UI를 띄워줌.
    public override void Interact()
    {
        base.Interact();

        _customizeNPCMenu.SetActive(true);
    }
}
