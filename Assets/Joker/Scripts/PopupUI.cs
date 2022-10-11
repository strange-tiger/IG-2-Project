using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button _closeButton;

    protected void OnEnable()
    {
        _closeButton.onClick.RemoveListener(Close);
        _closeButton.onClick.AddListener(Close);
    }

    protected void Close() => gameObject.SetActive(false);

    protected void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }
}
