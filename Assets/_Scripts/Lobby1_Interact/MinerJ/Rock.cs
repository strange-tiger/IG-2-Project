using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private Transform _miningUI;
    [SerializeField] private GameObject _miningButton;
    private BasicPlayerNetworking _player;
    public BasicPlayerNetworking Player { get { return _player; } }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerBody"))
        {
            // 획득한 재화를 DB에 저장하기 위해 Coin UI에 넘길 정보를 미리 저장해둠
            // Clone들의 경우 BasicPlayerNetworking가 없어 Null에러가 뜨기 때문에 예외처리 해줌
            _player = other.gameObject.transform.root.GetComponent<BasicPlayerNetworking>();
            if(_player == null)
            {
                return;
            }
            _miningButton.SetActive(true);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerBody"))
        {
            MiningUIDisable();
            _player = null;
        }
    }

    /// <summary>
    /// 지금까지 켜져있는 Mineral 관련 UI 다 꺼줌
    /// </summary>
    public void MiningUIDisable()
    {
        for(int i = 0; i < _miningUI.childCount; i++)
        {
            _miningUI.GetChild(i).gameObject.SetActive(false);
        }
    }
}
