using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu6_WeaponRack : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] Transform[] _weapons;

    private const string WEAPON_TAG = "IsekaiWeapon";

    public Vector3[] InitWeaponPositions { get; private set; }

    private static readonly WaitForSeconds SPAWN_DELAY = new WaitForSeconds(0.5f);

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

    private void Start()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(WEAPON_TAG))
        {
            return;
        }

        int currentGroupIndex = 0;

        for (int i = 0; i < _weapons.Length; ++i)
        {
            if (other.transform.IsChildOf(_weapons[i]))
            {
                currentGroupIndex = i;
                ++_weaponIndexGroup[currentGroupIndex];

                break;
            }
        }

        if (_weaponIndexGroup[currentGroupIndex] >= _weaponMaxIndexGroup[currentGroupIndex])
        {
            _weaponIndexGroup[currentGroupIndex] = 0;
        }

        SpawnWeaponRPCHelper(currentGroupIndex);
    }

    private void SpawnWeaponRPCHelper(int index) => StartCoroutine(SpawnWeapon(index));

    private IEnumerator SpawnWeapon(int groupIndex)
    {
        yield return SPAWN_DELAY;

        _weapons[groupIndex].GetChild(_weaponIndexGroup[groupIndex]).gameObject.SetActive(true);
    }
}
