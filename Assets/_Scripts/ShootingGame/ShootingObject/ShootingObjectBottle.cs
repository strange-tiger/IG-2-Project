using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObjectBottle : MonoBehaviour
{
    [SerializeField] private GameObject[] _bottles;

    private void Awake()
    {
        int bottleNumber = Random.Range(0, _bottles.Length);

        for(int i = 0; i<_bottles.Length; ++i)
        {
            _bottles[i].SetActive(bottleNumber == i);
        }
    }
}
