using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using TMPro;

public class GoldPanelManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _myGold;
    private PlayerNetworking _playerNetworking;
    private void Awake()
    {
        _playerNetworking = transform.root.GetComponent<PlayerNetworking>();
        _myGold.text = MySqlSetting.CheckHaveGold(_playerNetworking.MyNickname).ToString();
    }
}
