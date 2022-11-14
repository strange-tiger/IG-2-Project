using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Asset.MySql;

public class UserInteraction : InteracterableObject
{
    private PlayerNetworking _playerInfo;
    public GameObject RequestAlarmImage { private get; set; }

    public string Nickname { get; private set; }

    private bool _hasNickname;

    public override void Interact()
    {
        CheckAndGetMyNickname();
        MenuUIManager.Instance.ShowSocialUI(this);
    }

    [PunRPC]
    public void SendRequest(string requesterNickname)
    {
        CheckAndGetMyNickname();

        if(photonView.IsMine)
        {
            if(Nickname == requesterNickname)
            {
                return;
            }

            // ���� ����� ��밡 ģ�� �߰��� �ߴٸ� ������
            bool isLeft;
            byte blockByte;
            int relationship = MySqlSetting.CheckRelationship(Nickname, requesterNickname, out isLeft);
            if (isLeft)
            {
                blockByte = MySqlSetting._BLOCK_LEFT_BIT;
            }
            else
            {
                blockByte = MySqlSetting._BLOCK_RIGHT_BIT;
            }

            if ((relationship & blockByte) == blockByte)
            {
                return;
            }
            
            // �̹� ģ�� �߰� ��û�� �� �ִٸ� ������
            if (RequestAlarmImage.activeSelf)
            {
                return;
            }

            RequestAlarmImage.SetActive(true);
        }
    }

    private void CheckAndGetMyNickname()
    {
        if (!_hasNickname)
        {
            _playerInfo = GetComponent<PlayerNetworking>();
            Nickname = _playerInfo.MyNickname;
            _hasNickname = true;
        }
    }
}
