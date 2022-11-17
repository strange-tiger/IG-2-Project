using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using _DB = Asset.MySql.MySqlSetting;
using Photon.Pun.Demo.Cockpit;

public class RoomDestroyer : MonoBehaviourPunCallbacks
{
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 0)
        {
            _DB.DeleteRowByComparator(Asset.EroomlistdbColumns.UserID, $"{PhotonNetwork.CurrentRoom.Name}");
        }
    }
}
