using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        _text.text = "Clicked";
    }
}
