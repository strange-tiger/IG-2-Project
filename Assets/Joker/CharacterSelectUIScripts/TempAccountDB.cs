using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAccountDB : SingletonBehaviour<TempAccountDB>
{
    static public string ID { get; private set; }
    static public string Nickname { get; private set; }

    private void Start()
    {
        SetAccountData("", "");
    }

    static public void SetAccountData(string id, string nickname)
    {
        ID = id;
        Nickname = nickname;
    }
}
