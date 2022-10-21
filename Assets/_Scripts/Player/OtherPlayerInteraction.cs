using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerInteraction : InteracterableObject
{
    public string MyNickname { private get; set; }

    private void OnEnable()
    {
        MyNickname = GetComponent<PlayerNetworking>().MyNickname;
    }

    public override void Interact()
    {
        PlayerMenuUIManager.Instance.ShowSocial(MyNickname);
    }
}