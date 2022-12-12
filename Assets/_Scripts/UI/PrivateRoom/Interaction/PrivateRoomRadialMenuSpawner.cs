using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PrivateRoomRadialMenuSpawner : MonoBehaviourPunCallbacks
{
    private static readonly YieldInstruction MENU_DELAY = new WaitForSeconds(1f);
    private List<Canvas> _menuList = new List<Canvas>();

    public override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(SpawnDelay());
    }

    /// <summary>
    /// MENU_DELAY의 지연 이후 사설 공간 메뉴를 생성한다.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnDelay()
    {
        yield return MENU_DELAY;

        GameObject newMenu = PhotonNetwork.Instantiate("PrivateRoom\\NEW_PrivateRoomRadialMenuCanvas", Vector3.zero, Quaternion.identity);
        _menuList.Add(newMenu.GetComponent<Canvas>());
    }

    /// <summary>
    /// 유저가 룸에서 나가면 그 외 월드에서 호출된다.
    /// 생성했던 메뉴를 삭제한다.
    /// </summary>
    /// <param name="otherPlayer"></param>
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
