using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using _DB = Asset.MySql.MySqlSetting;

public class RoomDestroyer : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// 해당 오브젝트가 삭제될 때 호출된다.
    /// 현재 방에 있는 PlayerCount가 1보다 작을 떄, DB에서 현재 방의 정보를 삭제한다.
    /// </summary>
    private void OnDestroy()
    {
        if ((int)PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            _DB.DeleteRowByComparator(Asset.EroomlistdbColumns.UserID, $"{PhotonNetwork.CurrentRoom.Name}");
        }
    }
}
