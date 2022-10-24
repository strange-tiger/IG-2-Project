using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomInfoTextUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _text;

    [Header("Lock")]
    [SerializeField] GameObject _lockImage;

    private string _roomInfo = "";
    private bool _isLocked = false;

    private void Awake()
    {
        _lockImage.SetActive(false);
    }

    private void OnEnable()
    {
        UpdateRoomInfo();
    }

    public void UpdateRoomInfo()
    {
        _text.gameObject.SetActive(true);
        _text.text = _roomInfo;
        _lockImage.SetActive(_isLocked);
    }

    public void SetInfo(string info)
    {
        _roomInfo = info;
    }

    public void SetLock(bool isLocked)
    {
        _isLocked = isLocked;
    }
}
