using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateRoomExit : ServerChange
{
    protected override string CheckMessage()
    {
        return "���� �����ðڽ��ϱ�?";
    }
}
