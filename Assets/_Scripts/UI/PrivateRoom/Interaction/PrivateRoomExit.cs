using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateRoomExit : ServerChange
{
    protected override string CheckMessage()
    {
        return "정말 나가시겠습니까?";
    }
}
