using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using _DB = Asset.MySql.MySqlSetting;

public class RoomDestroyer : MonoBehaviourPunCallbacks
{
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            _DB.DeleteRowByComparator(Asset.EroomlistdbColumns.UserID, $"{PhotonNetwork.CurrentRoom.Name}");
        }
    }
}
