using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.EPetUIIndex;

public class PetUIManager : UIManager
{
    [SerializeField] Collider _npcCollider;

    private void Awake()
    {
        ShutPetUI();
    }

    public void LoadUI(_UI ui)
    {
        _npcCollider.enabled = false;

        LoadUI((int)ui);
    }

    public void ShutPetUI()
    {
        ShutUI();

        _npcCollider.enabled = true;
    }
}
