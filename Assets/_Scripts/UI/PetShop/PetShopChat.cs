using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _UI = Defines.EPetShopUIIndex;

public class PetShopChat : MonoBehaviour
{
    [Header("UIManager")]
    [SerializeField] PetShopUIManager _ui;

    private void OnEnable()
    {
        StartCoroutine(Conversation());
    }

    private IEnumerator Conversation()
    {
        bool hasNoConversation = true;
        while (hasNoConversation)
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                hasNoConversation = false;
            }

            yield return null;
        }

        _ui.LoadUI(_UI.FIRST);
    }
}
