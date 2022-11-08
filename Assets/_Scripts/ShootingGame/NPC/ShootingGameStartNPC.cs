using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootingGameStartNPC : InteracterableObject
{
    private const int _MAX_MAP_COUNT = 100;

    [SerializeField] private float _mapXOffset = 1000f;
    private List<string> _playerList = new List<string>();
    //private List<ShootingSearchPartyPlayer> _playerList = new List<ShootingSearchPartyPlayer>();

    private bool[] _isThereMap = new bool[_MAX_MAP_COUNT];
    private Queue<GameObject> _mapQueue = new Queue<GameObject>();

    private bool _isPlayer

    public override void Interact()
    {
        MenuUIManager.Instance.ShowCheckPanel("Play?",
            () => {
                ShootingSearchPartyPlayer player = MenuUIManager.Instance.gameObject.GetComponentInChildren<ShootingSearchPartyPlayer>();
                player.ShowSearchUI(this);
                photonView.RPC("AddPlayerToList", RpcTarget.AllViaServer, player);
            },
            () => { }
            );
    }

    [PunRPC]
    private void AddPlayerToList(ShootingSearchPartyPlayer player)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _playerList.Add(player);
            SetGameAndStart();
        }
    }

    private void SetGameAndStart()
    {
        if(_playerList.Count < ShootingGameManager._MAX_PLAYER_COUNT)
        {
            return;
        }

        for (int i = 0; i < _MAX_MAP_COUNT; ++i)
        {
            if (!_isThereMap[i])
            {
                GameObject newMap = PhotonNetwork.Instantiate("ShootingRange", new Vector3(_mapXOffset * (i + 1), 0f, 0f), Quaternion.identity);
                ShootingGameManager newShootingGM = newMap.GetComponent<ShootingGameManager>();
                _mapQueue.Enqueue(newMap);
                _isThereMap[i] = true;

                ShootingGameManager shootingGameManager = newMap.GetComponent<ShootingGameManager>();

                List<GameObject> playerList = new List<GameObject>();
                for(int j = 0; j < _MAX_MAP_COUNT; ++j)
                {
                    _playerList[j].FoundParty(newShootingGM);
                    playerList.Add(_playerList[j].transform.root.gameObject);
                    _playerList.RemoveAt(0);
                }

                shootingGameManager.StartGame(playerList);
                break;
            }
        }
    }

    [PunRPC]
    private void RemovePlayerInList(ShootingSearchPartyPlayer player)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (_playerList.Contains(player))
            {
                _playerList.Remove(player);
            }
        }
    }

    public void CancelSearching(ShootingSearchPartyPlayer player)
    {
        photonView.RPC("RemovePlayerInList", RpcTarget.AllViaServer, player);
    }
}
