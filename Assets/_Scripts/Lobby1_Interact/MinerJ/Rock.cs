using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private Transform _miningUI;
    [SerializeField]
    private GameObject _miningButton;
    private BasicPlayerNetworking _player;
    public BasicPlayerNetworking Player { get { return _player; } }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerBody"))
        {
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

    public void MiningUIDisable()
    {
        for(int i = 0; i < _miningUI.childCount; i++)
        {
            _miningUI.GetChild(i).gameObject.SetActive(false);
        }
    }
}
