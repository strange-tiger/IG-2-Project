using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class CustomizeNPC : InteracterableObject
{
    // NPC에게 상호작용 했을 때 띄울 UI.
    [SerializeField] private GameObject _customizeNPCMenu;

    /// <summary>
    /// 커스터마이징 NPC에게 상호작용하면 UI를 띄워줌.
    /// </summary>
    public override void Interact()
    {
        _customizeNPCMenu.SetActive(true);
    }
}
