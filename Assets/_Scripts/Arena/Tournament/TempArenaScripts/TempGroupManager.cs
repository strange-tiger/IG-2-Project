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
    /// 부전승 처리
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
    /// 토너먼트 참가자 셋팅
    /// </summary>
    private void GroupTournamentSet()
    {
        // 참가한 챔피언의 수
        int championsNum = transform.childCount;

        // 챔피언들을 랜덤하게 배치하기 위한 랜덤값 넣어주기
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
    /// 전투 시작위치 셋팅
    /// </summary>
    /// <param name="list">참가시킬 리스트</param>
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
    /// 전투 코루틴
    /// </summary>
    /// <param name="list">전투 진행시킬 리스트</param>
    /// <param name="winnerList">승리자를 담을 리스트</param>
    /// <returns></returns>
    private IEnumerator BattleStart(List<GameObject> list, List<GameObject> winnerList)
    {
        WaitUntil waitUntilList = new WaitUntil(() => SomeOneDie(list, winnerList));
        WaitUntil waitUntilWinnerList = new WaitUntil(() => SomeOneDie(winnerList, list));

        while (true)
        {
            if (Any(list))
            {
                // 부전승 판정들
                LuckyWinnerSet(winnerList);

                BattlePositionAndRotationSet(winnerList);

                // 경기 진행
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
                // 부전승 판정들
                LuckyWinnerSet(list);

                BattlePositionAndRotationSet(list);

                // 경기 진행
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
    /// 리스트가 비었으면 true 아니면 false
    /// </summary>
    /// <param name="list">확인할 리스트</param>
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
    /// 다음 전투 진행시킬 메서드
    /// </summary>
    /// <param name="fightGroup">전투시킬 리스트 넣어주기</param>
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
    /// 경기 중 누군가 죽었을 때 이루어질 동작들
    /// </summary>
    /// <param name="fightGroup">싸우고있는 그룹</param>
    /// <param name="winerGroup">승자들을 넣어줄 그룹</param>
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
    /// 경기종료 액션이벤트
    /// </summary>
    private void BattleEnd()
    {
        _battleValue = false;
    }
}
