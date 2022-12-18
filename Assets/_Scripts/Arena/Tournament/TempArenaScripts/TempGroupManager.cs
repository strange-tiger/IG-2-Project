using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TempGroupManager : MonoBehaviour
{
    private List<int> _groupList = new List<int>();
    private List<GameObject> _champions = new List<GameObject>();
    private List<GameObject> _winerChampins = new List<GameObject>();
    private List<GameObject> _luckyGroup = new List<GameObject>();
    
    private WaitForSeconds _waitTime = new WaitForSeconds(2f);

    private Action _battleEnd;

    private IEnumerator _InextBattle;

    private Vector3 _evenChampionPostion = new Vector3(-8.5f, 0f, 0f);
    private Vector3 _evenChampionRotation = new Vector3(0f, 90f, 0f);
    
    private Vector3 _oddChampionPostion = new Vector3(8.5f, 0f, 0f);
    private Vector3 _oddChampionRotation = new Vector3(0f, -90f, 0f);

    private bool _battleValue = true;

    private const int _redTeamIndex = 0;
    private const int _blueTeamIndex = 1;
    private const int _luckyWinnerIndex = 0;

    private int _randGroupIndex;

    private void Awake()
    {
        GroupTournamentSet();
    }

    private void Start()
    {
        _InextBattle = BattleStart(_champions, _winerChampins);
        StartCoroutine(_InextBattle);

        _battleEnd = BattleEnd;
    }

    /// <summary>
    /// ������ ó��
    /// </summary>
    private void LuckyWinnerSet(List<GameObject> list)
    {
        int championsNum = list.Count;
        int championsIndex = championsNum - 1;

        if (championsNum % 2 != 0)
        {
            if (!IsEmpty(_luckyGroup))
            {
                list.Add(_luckyGroup[_luckyWinnerIndex]);
                _luckyGroup.Clear();
                return;
            }
            _luckyGroup.Add(list[championsIndex]);
            list.RemoveAt(championsIndex);
        }
    }

    /// <summary>
    /// ��ʸ�Ʈ ������ ����
    /// </summary>
    private void GroupTournamentSet()
    {
        // ������ è�Ǿ��� ��
        int championsNum = transform.childCount;

        // è�Ǿ���� �����ϰ� ��ġ�ϱ� ���� ������ �־��ֱ�
        for (int i = 0; i < transform.childCount; ++i)
        {
            _randGroupIndex = UnityEngine.Random.Range(0, championsNum);

            for (int j = 0; j < _groupList.Count;)
            {
                if (_groupList[j] == _randGroupIndex)
                {
                    j = 0;
                    _randGroupIndex = UnityEngine.Random.Range(0, championsNum);
                }

                else
                {
                    ++j;
                }
            }
            _groupList.Add(_randGroupIndex);
            _champions.Add(transform.GetChild(_groupList[i]).gameObject);
        }
    }

    /// <summary>
    /// ���� ������ġ ����
    /// </summary>
    /// <param name="list">������ų ����Ʈ</param>
    private void BattlePositionAndRotationSet(List<GameObject> list)
    {
        int even = 0;
        int odd = 1;
        for (int i = 0; i < list.Count * 0.5; ++i)
        {
            list[even].transform.position = _evenChampionPostion;
            list[even].transform.rotation = Quaternion.Euler(_evenChampionRotation);

            list[odd].transform.position = _oddChampionPostion;
            list[odd].transform.rotation = Quaternion.Euler(_oddChampionRotation);

            even += 2;
            odd += 2;
        }
    }

    /// <summary>
    /// ���� �ڷ�ƾ
    /// </summary>
    /// <param name="list">���� �����ų ����Ʈ</param>
    /// <param name="winnerList">�¸��ڸ� ���� ����Ʈ</param>
    /// <returns></returns>
    private IEnumerator BattleStart(List<GameObject> list, List<GameObject> winnerList)
    {
        WaitUntil waitUntilList = new WaitUntil(() => SomeOneDie(list, winnerList));
        WaitUntil waitUntilWinnerList = new WaitUntil(() => SomeOneDie(winnerList, list));

        while (true)
        {
            List<GameObject> nowList = IsEmpty(list) ? winnerList : list;
            List<GameObject> nextList = IsEmpty(list) ? list : winnerList;
            WaitUntil waitUntil = IsEmpty(list) ? waitUntilWinnerList : waitUntilList;

            // ������ ������
            LuckyWinnerSet(nowList);

            BattlePositionAndRotationSet(nowList);

            // ��� ����
            while (true)
            {
                FightGroup(nowList);
                if (!_battleValue)
                {
                    break;
                }
                yield return waitUntil;
            }
            yield return _waitTime;

            if (nextList.Count > 1)
            {
                _battleValue = true;
            }
            else if (nextList.Count == 1 && !IsEmpty(_luckyGroup))
            {
                continue;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// ����Ʈ�� ������� true �ƴϸ� false
    /// </summary>
    /// <param name="list">Ȯ���� ����Ʈ</param>
    /// <returns></returns>
    private bool IsEmpty(List<GameObject> list)
    {
        if (list.Count == 0)
        {
            return true;

        }
        return false;
    }

    /// <summary>
    /// ���� ���� �����ų �޼���
    /// </summary>
    /// <param name="fightGroup">������ų ����Ʈ �־��ֱ�</param>
    private void FightGroup(List<GameObject> fightGroup)
    {
        if (!IsEmpty(fightGroup))
        {
            fightGroup[_redTeamIndex].SetActive(true);
            fightGroup[_blueTeamIndex].SetActive(true);
        }
        else
        {
            _battleEnd?.Invoke();
        }
    }

    /// <summary>
    /// ��� �� ������ �׾��� �� �̷���� ���۵�
    /// </summary>
    /// <param name="fightGroup">�ο���ִ� �׷�</param>
    /// <param name="winnerGroup">���ڵ��� �־��� �׷�</param>
    /// <returns></returns>
    private bool SomeOneDie(List<GameObject> fightGroup, List<GameObject> winnerGroup)
    {
        if (fightGroup.Count < 2)
        {
            return false;
        }

        if (fightGroup[_redTeamIndex].activeSelf == false || fightGroup[_blueTeamIndex].activeSelf == false)
        {
            int index = fightGroup[_redTeamIndex].activeSelf == false ? _blueTeamIndex : _redTeamIndex;
            winnerGroup.Add(fightGroup[index]);
            fightGroup[index].SetActive(false);
            fightGroup.RemoveRange(0, 2);

            return true;
        }

        return false;
    }

    /// <summary>
    /// ������� �׼��̺�Ʈ
    /// </summary>
    private void BattleEnd()
    {
        _battleValue = false;
    }
}
