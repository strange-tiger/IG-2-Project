using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using TMPro;

public class GoldPanelManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _myGold;
    [SerializeField] GameObject _goldPanel;
    private BasicPlayerNetworking _playerNetworking;
    private bool _isGoldUpdateComplete;
    private void Awake()
    {
        StartCoroutine(BringMyNickname());
    }

    IEnumerator BringMyNickname()
    {
        yield return new WaitForSeconds(3f);

        _playerNetworking = transform.root.GetComponent<BasicPlayerNetworking>();
    }

    private void Update()
    {
        if(_goldPanel.activeSelf)
        {
            if (_isGoldUpdateComplete == false)
            {
                _myGold.text = MySqlSetting.CheckHaveGold(_playerNetworking.MyNickname).ToString();
            }
            _isGoldUpdateComplete = true;
        }
        else
        {
            _isGoldUpdateComplete = false;
        }
    }
}
