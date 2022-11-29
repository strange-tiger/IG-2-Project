using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _DB = Asset.MySql.MySqlSetting;
using _IRM = Defines.RPC.IsekaiRPCMethodName;
using UnityEngine.SceneManagement;

public class Tu6_SummonCircle : MonoBehaviour
{
    [Header("Isekai Objects")]
    [SerializeField] Tu6_IsekaiObject[] _objects;

    [Header("UI")]
    [SerializeField] GameObject _goldUI;

    [Header("Audio")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _fallAudioClip;
    [SerializeField] AudioClip _riseAudioClip;

    private static readonly WaitForSeconds CONGRAT_DELAY = new WaitForSeconds(0.5f);
    private static readonly WaitForSeconds SPAWN_DELAY = new WaitForSeconds(1f);
    
    private static readonly Vector3 FLOAT_POSITION = new Vector3(0f, 1.2f, 0f);
    private static readonly Vector3 WAIT_POSITION = new Vector3(0f, -1.5f, 0f);
    private const float RISE_TIME = 1f;
    private const int MAX_TO_HIT = 101;
    private const int PERCENT_TO_POINT = 1; // 원하는 % 수
    private const int EARN_GOLD = 500;

    private BasicPlayerNetworking _playerNetworking;
    private Vector3 _playerPosition = new Vector3();

    private void OnEnable()
    {
        foreach (Tu6_IsekaiObject obj in _objects)
        {
            obj.ObjectSlashed -= SpawnRPCHelper;
            obj.ObjectSlashed += SpawnRPCHelper;

            obj.ObjectSlashed -= GetGold;
            obj.ObjectSlashed += GetGold;

            obj.gameObject.SetActive(false);
        }

        _audioSource.volume = 0f;

        SpawnRPCHelper(_playerPosition);

        _goldUI.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (Tu6_IsekaiObject obj in _objects)
        {
            obj.ObjectSlashed -= SpawnRPCHelper;
            obj.ObjectSlashed -= GetGold;
        }
    }

    private void SpawnRPCHelper(Vector3 playerPos)
    {
        _currentIndex = Random.Range(0, _objects.Length);

        SpawnHelper(_currentIndex);
    }

    private void SpawnHelper(int currentIndex) => StartCoroutine(SpawnObject(currentIndex));

    private int _currentIndex = 0;
    private float _elapsedTime = 0f;
    private IEnumerator SpawnObject(int currentIndex)
    {
        _audioSource.PlayOneShot(_fallAudioClip);

        yield return SPAWN_DELAY;

        _audioSource.PlayOneShot(_riseAudioClip);

        _objects[currentIndex].gameObject.SetActive(true);

        while (_elapsedTime <= RISE_TIME)
        {
            _elapsedTime += Time.deltaTime;

            _objects[currentIndex].transform.localPosition = Vector3.Lerp(WAIT_POSITION, FLOAT_POSITION, _elapsedTime);

            yield return null;
        }

        _elapsedTime = 0f;

        _objects[currentIndex].ReturnIsNotFlick();
    }


    private void GetGold(Vector3 playerPos)
    {
        if (PERCENT_TO_POINT < Random.Range(1, MAX_TO_HIT))
        {
            return;
        }

        _DB.EarnGold(_playerNetworking.MyNickname, EARN_GOLD);

        StartCoroutine(ShowGoldUI(playerPos));
    }

    private IEnumerator ShowGoldUI(Vector3 playerPos)
    {
        yield return CONGRAT_DELAY;

        _audioSource.PlayOneShot(_audioSource.clip);

        _goldUI.transform.LookAt(playerPos, Vector3.up);

        _goldUI.SetActive(true);

        yield return SPAWN_DELAY;

        _goldUI.SetActive(false);
    }
}
