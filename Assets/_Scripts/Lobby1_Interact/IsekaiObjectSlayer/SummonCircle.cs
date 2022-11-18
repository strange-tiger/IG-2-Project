#define debug
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using _DB = Asset.MySql.MySqlSetting;

public class SummonCircle : MonoBehaviourPun
{
    [Header("Isekai Objects")]
    [SerializeField] IsekaiObject[] _objects;

    [Header("UI")]
    [SerializeField] GameObject _goldUI;

    private static readonly WaitForSeconds SPAWN_DELAY = new WaitForSeconds(1f);
    private static readonly Vector3 FLOAT_POSITION = new Vector3(0f, 1.2f, 0f);
    private static readonly Vector3 WAIT_POSITION = new Vector3(0f, -0.5f, 0f);
    private const float RISE_TIME = 1f;
    private const int MAX_TO_HIT = 100;
    private const int PERCENT_TO_POINT = 0;
    private const int EARN_GOLD = 500;


    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;
    private Vector3 _playerPosition = new Vector3();

    private void OnEnable()
    {

        foreach (IsekaiObject obj in _objects)
        {
            obj.ObjectSlashed -= SpawnRPCHelper;
            obj.ObjectSlashed += SpawnRPCHelper;

            obj.gameObject.SetActive(false);
        }
        
        SpawnRPCHelper(_playerPosition);
      
        _goldUI.SetActive(false);

        StartCoroutine(SetPlayerNetworking());
    }

    private void OnDisable()
    {
        foreach (IsekaiObject obj in _objects)
        {
            obj.ObjectSlashed -= SpawnRPCHelper;
            obj.ObjectSlashed -= GetGold;
        }
    }

    private IEnumerator SetPlayerNetworking()
    {
        yield return new WaitForSeconds(3f);

        _playerNetworkings = FindObjectsOfType<BasicPlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }
    }

    private void SpawnRPCHelper(Vector3 playerPos)
    {
        _currentIndex = Random.Range(0, _objects.Length);

#if debug
        SpawnHelper(_currentIndex);
#else
        photonView.RPC("SpawnHelper", RpcTarget.AllBuffered, _currentIndex);
#endif
    }

    private void SpawnHelper(int currentIndex) => StartCoroutine(SpawnObject(currentIndex));

    private int _currentIndex = 0;
    private float _elapsedTime = 0f;
    private IEnumerator SpawnObject(int currentIndex)
    {
        yield return SPAWN_DELAY;

        

        _objects[currentIndex].gameObject.SetActive(true);
        
        while (_elapsedTime <= RISE_TIME)
        {
            _elapsedTime += Time.deltaTime;
            
            _objects[currentIndex].transform.localPosition = Vector3.Lerp(WAIT_POSITION, FLOAT_POSITION, _elapsedTime);

            yield return null;
        }

        _elapsedTime = 0f;
    }

    
    private void GetGold(Vector3 playerPos)
    {
        if (PERCENT_TO_POINT < Random.Range(0, MAX_TO_HIT))
        {
            return;
        }
#if !debug

        _DB.EarnGold(_playerNetworking.MyNickname, EARN_GOLD);
#endif

        StartCoroutine(ShowGoldUI(playerPos));
    }

    private IEnumerator ShowGoldUI(Vector3 playerPos)
    {
        _goldUI.transform.LookAt(playerPos);

        _goldUI.SetActive(true);

        yield return SPAWN_DELAY;

        _goldUI.SetActive(false);
    }
}
