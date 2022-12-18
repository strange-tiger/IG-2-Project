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

    private const int _zero = 0;
    private const int _one = 1;
    private const int _two = 2;
    private const float _half = 0.5f;

    private static bool _battleValue = true;

    private int _randGroupIndex;

    private int _evenIndex = 0;
    private int _oddIndex = 1;

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
        int championsIndex = championsNum - _one;

        if (championsNum % _two != _zero)
        {
            if (!Any(_luckyGroup))
            {
                list.Add(_luckyGroup[_zero]);
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
        for (int i = _zero; i < transform.childCount; ++i)
        {
            _randGroupIndex = UnityEngine.Random.Range(_zero, championsNum);

            for (int j = _zero; j < _groupList.Count;)
            {
                if (_groupList[j] == _randGroupIndex)
                {
                    j = _zero;
                    _randGroupIndex = UnityEngine.Random.Range(_zero, championsNum);
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
        for (int i = _zero; i < list.Count * _half; ++i)
        {
            list[even].transform.position = _evenChampionPostion;
            list[even].transform.rotation = Quaternion.Euler(_evenChampionRotation);

            list[odd].transform.position = _oddChampionPostion;
            list[odd].transform.rotation = Quaternion.Euler(_oddChampionRotation);

            even += _two;
            odd += _two;
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
            if (Any(list))
            {
                // ������ ������
                LuckyWinnerSet(winnerList);

                BattlePositionAndRotationSet(winnerList);

                // ��� ����
                while (true)
                {
                    FightGroup(winnerList);
                    if (!_battleValue)
                    {
                        break;
                    }
                    yield return waitUntilWinnerList;
                }
                yield return _waitTime;

                if (list.Count > _one)
                {
                    _battleValue = true;
                }
                else if (list.Count == _one && !Any(_luckyGroup))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            else
            {
                // ������ ������
                LuckyWinnerSet(list);

                BattlePositionAndRotationSet(list);

                // ��� ����
                while (true)
                {
                    FightGroup(list);
                    if (!_battleValue)
                    {
                        break;
                    }
                    yield return waitUntilList;
                }
                yield return _waitTime;

                if (winnerList.Count > _one)
                {
                    _battleValue = true;
                }
                else if (winnerList.Count == _one && !Any(_luckyGroup))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }  
        }
    }

    /// <summary>
    /// ����Ʈ�� ������� true �ƴϸ� false
    /// </summary>
    /// <param name="list">Ȯ���� ����Ʈ</param>
    /// <returns></returns>
    private bool Any(List<GameObject> list)
    {
        if (list.Count == _zero)
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
        if (!Any(fightGroup))
        {
            fightGroup[_evenIndex].SetActive(true);
            fightGroup[_oddIndex].SetActive(true);
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
    /// <param name="winerGroup">���ڵ��� �־��� �׷�</param>
    /// <returns></returns>
    private bool SomeOneDie(List<GameObject> fightGroup, List<GameObject> winerGroup)
    {
        if (_evenIndex == fightGroup.Count || _oddIndex == fightGroup.Count)
        {
            return false;
        }

        if (fightGroup[_evenIndex].activeSelf == false || fightGroup[_oddIndex].activeSelf == false)
        {
            if (fightGroup[_evenIndex].activeSelf == false)
            {
                winerGroup.Add(fightGroup[_oddIndex]);
                fightGroup[_oddIndex].SetActive(false);
                fightGroup.RemoveAt(_zero);
                fightGroup.RemoveAt(_zero);

                return true;
            }
            else if (fightGroup[_oddIndex].activeSelf == false)
            {
                winerGroup.Add(fightGroup[_evenIndex]);
                fightGroup[_evenIndex].SetActive(false);
                fightGroup.RemoveAt(_zero);
                fightGroup.RemoveAt(_zero);

                return true;
            }
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
