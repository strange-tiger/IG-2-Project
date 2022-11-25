using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PrivateRoomRadialMenuSpawner : MonoBehaviourPunCallbacks
{
    private static readonly YieldInstruction MENU_DELAY = new WaitForSeconds(1f);
    private List<Canvas> _menuList;

    public override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnDelay()
    {
        yield return MENU_DELAY;

        GameObject newMenu = PhotonNetwork.Instantiate("PrivateRoom\\NEW_PrivateRoomRadialMenuCanvas", Vector3.zero, Quaternion.identity);
        _menuList.Add(newMenu.GetComponent<Canvas>());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        foreach (Canvas menu in _menuList)
        {
            if (menu.worldCamera == null)
            {
                _menuList.Remove(menu);
                PhotonNetwork.Destroy(menu.gameObject);
                break;
            }
        }
    }
}
