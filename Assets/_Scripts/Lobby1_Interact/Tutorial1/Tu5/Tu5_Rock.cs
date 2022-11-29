using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu5_Rock : MonoBehaviour
{
    [SerializeField]
    private Transform _miningUI;
    [SerializeField]
    private GameObject _miningButton;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            _miningButton.SetActive(true);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            MiningUIDisable();
        }
    }

    public void MiningUIDisable()
    {
        for (int i = 0; i < _miningUI.childCount; i++)
        {
            _miningUI.GetChild(i).gameObject.SetActive(false);
        }
    }
}
