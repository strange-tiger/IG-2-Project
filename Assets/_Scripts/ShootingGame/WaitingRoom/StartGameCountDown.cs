using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartGameCountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countDownText;

    public void SetCountDownText(string countDown)
    {
        _countDownText.text = countDown;
    }
}
