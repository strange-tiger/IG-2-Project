using System.Collections;
using UnityEngine;
using Photon.Pun;

public class WeaponRack : MonoBehaviourPun
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

    /// <summary>
    /// 무기가 무기 보관대로부터 일정 거리 이상 벗어나면 새로운 무기를 소환한다.
    /// 
    /// 태그가 "IsekaiWeapon"인 콜라이더와의 트리거 충돌에서 벗어나면 ObjectPoolWeapons를 호출한다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(WEAPON_TAG))
        {
            return;
        }

        ObjectPoolWeapons(other.transform);
    }

    /// <summary>
    /// 각 무기 그룹의 몇 번째 무기를 소환할 지 결정한다.
    /// 
    /// 태그가 "IsekaiWeapon"인 콜라이더와의 트리거 충돌에서 벗어나면 호출된다.
    /// Collider 변수 weapon을 매개변수로 전달 받는다.
    /// weapon이 무기의 풀 _weapons 중 하나의 자식인지 판별하여,
    /// currentGroupIndex에 weapon의 부모가 _weapons의 몇 번째 요소인지 그 인덱스 값을 기억한다.
    /// _weaponIndexGroup에 저장한 각 _weapons의 
    /// </summary>
    /// <param name="weapon"></param>
    private void ObjectPoolWeapons(Transform weapon)
    {
        int currentGroupIndex = 0;

        for (int i = 0; i < _weapons.Length; ++i)
        {
            if (weapon.IsChildOf(_weapons[i]))
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

        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(SpawnWeaponRPCHelper));
        photonView.RPC(nameof(SpawnWeaponRPCHelper), RpcTarget.AllBuffered, currentGroupIndex);
    }

    [PunRPC]
    private void SpawnWeaponRPCHelper(int index) => StartCoroutine(SpawnWeapon(index));

    /// <summary>
    /// 0.5초 후 무기를 소환한다.
    /// 
    /// 호출되면 SPAWN_DELAY(0.5초)의 딜레이 후 매개변수 groupIndex에 따라 _weapons 요소의 자식 _weaponIndexGroup 번째 오브젝트를 활성화한다.
    /// </summary>
    /// <param name="groupIndex"></param>
    /// <returns></returns>
    private IEnumerator SpawnWeapon(int groupIndex)
    {
        yield return SPAWN_DELAY;

        _weapons[groupIndex].GetChild(_weaponIndexGroup[groupIndex]).gameObject.SetActive(true);
    }
}
