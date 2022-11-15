using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponRack : MonoBehaviourPun
{
    [Header("Weapons")]
    [SerializeField] Transform[] _weapons;

    public Vector3[] InitWeaponPositions { get; private set; }
    
    private int[] _weaponIndexGroup;
    private int[] _weaponMaxIndexGroup;

    private void Awake()
    {
        if (_weapons.Length == 0)
        {
            _weapons = new Transform[transform.childCount - 1];

            for (int i = 0; i < transform.childCount - 1; ++i)
            {
                _weapons[i] = transform.GetChild(i + 1);
            }
        }

        int weaponNum = _weapons.Length;
        InitWeaponPositions = new Vector3[weaponNum];
        _weaponIndexGroup = new int[weaponNum];
        _weaponMaxIndexGroup = new int[weaponNum];
        for (int i = 0; i < weaponNum; ++i)
        {
            InitWeaponPositions[i] = _weapons[i].position;
            _weaponIndexGroup[i] = 0;
            _weaponMaxIndexGroup[i] = _weapons[i].childCount;
        }
        
        foreach (Transform weapons in _weapons)
        {
            weapons.GetChild(0).gameObject.SetActive(true);

            for (int i = 1; i < weapons.childCount; ++i)
            {
                weapons.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("IsekaiWeapon"))
        {
            return;
        }

        int currentIndex = 0;
        for (int i = 0; i < _weapons.Length; ++i)
        {
            if (other.transform.IsChildOf(_weapons[i]))
            {
                ++_weaponIndexGroup[i];
                currentIndex = i;

                break;
            }
        }

        if (_weaponIndexGroup[currentIndex] >= _weaponMaxIndexGroup[currentIndex])
        {
            _weaponIndexGroup[currentIndex] = 0;
        }

        _weapons[currentIndex].GetChild(_weaponIndexGroup[currentIndex]).gameObject.SetActive(true);
    }
}
