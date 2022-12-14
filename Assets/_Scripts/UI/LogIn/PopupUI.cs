using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] protected Button _closeButton;

    protected virtual void OnEnable()
    {
        _closeButton.onClick.RemoveListener(Close);
        _closeButton.onClick.AddListener(Close);
    }

    /// <summary>
    /// PopupUI 오브젝트를 비활성화
    /// </summary>
    protected virtual void Close() => gameObject.SetActive(false);

    protected virtual void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }
}
