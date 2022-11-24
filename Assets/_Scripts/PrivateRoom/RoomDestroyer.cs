using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using _DB = Asset.MySql.MySqlSetting;

public class RoomDestroyer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("[����] " + PhotonNetwork.CurrentRoom.Name);
    }

    private void OnDestroy()
    {
        Debug.Log("[���� ��] " + PhotonNetwork.CurrentRoom.Name);

        if ((int)PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            Debug.Log("[���� ��] " + PhotonNetwork.CurrentRoom.Name);
            _DB.DeleteRowByComparator(Asset.EroomlistdbColumns.UserID, $"{PhotonNetwork.CurrentRoom.Name}");
        }
    }
}
