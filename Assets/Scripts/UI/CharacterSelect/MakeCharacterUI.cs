using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.ECharacterUIIndex;

public class MakeCharacterUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] CharacterUIManager _characterUIManager;

    [Header("Button")]
    [SerializeField] Button _backButton;

    private void OnEnable()
    {
        _backButton.onClick.RemoveListener(GoBack);
        
        _backButton.onClick.AddListener(GoBack);
    }

    private void GoBack() => _characterUIManager.LoadUI(_UI.SELECT);
}
