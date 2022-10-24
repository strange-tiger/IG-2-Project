using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomInfo : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _roomInfoText;

    [Header("Lock")]
    [SerializeField] GameObject _lockImage;

    private string _roomInfo;
    private bool _locked;

    private void Awake()
    {
        _lockImage.SetActive(false);
    }

    private void OnEnable()
    {
        _roomInfoText.gameObject.SetActive(true);
        _lockImage.SetActive(_locked);
    }

    public void LoadRoomInfo()
    {
        _roomInfoText.text = _roomInfo;
    }

    public void LockUpdate()
    {
        _locked = true;
    }
}
