using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using SceneNumber = Defines.ESceneNumber;
using Photon.Pun;

public class StartRoomTutorialCheck : MonoBehaviour
{
    [SerializeField] private ETutorialCompleteState _state;
    [SerializeField] private GameObject _tutorial;

    private void Awake()
    {
        StartRoomTutorial();
    }

    private void StartRoomTutorial()
    {
        if (MySqlSetting.CheckCompleteTutorial(PhotonNetwork.NickName, _state))
        {
            _tutorial.SetActive(false);
            return;
        }

        _tutorial.SetActive(true);
    }
}
