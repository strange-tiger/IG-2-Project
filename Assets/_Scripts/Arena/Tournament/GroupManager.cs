using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class GroupManager : MonoBehaviourPun
{
    // 챔피언의 초기 위치값
    [SerializeField] private int _setPositionX;
    [SerializeField] private float _setPositionZ;

    // 그룹 내 참가자들의 배열
    private GameObject[] _member = new GameObject[4];
    public GameObject[] Member { get { return _member; } }

    // 결승전에 들어갈 배열
    private GameObject[] _finalBattle = new GameObject[2];

    // 첫번째 준결승 위치 셋팅 해 주는 이벤트
    private UnityEvent _setFirstBattle = new UnityEvent();

    // 첫번째 준결승 이 종료되면 호출되는 이벤트
    private UnityEvent _FirstBattleWinnerSetFinalGroup = new UnityEvent();
    // 두번째 준결승 이 종료되면 호출되는 이벤트
    private UnityEvent _SecondBattleWinnerSetFinalGroup = new UnityEvent();
    // 베팅매니저와 연결되어있는 경기가 종료되면 호출되는 이벤트
    public UnityEvent _finishTournament = new UnityEvent();

    // 랜덤하게 결정되는 그룹 순서
    List<int> _memberIndexList;

    private bool _isFirstBattle;
    private bool _isSecondBattle;
    private bool _isFinelBattle;

    // 이긴 챔피언의 인덱스
    private int _winnerIndex;
    public int WinnerIndex { get { return _winnerIndex; } private set { _winnerIndex = value; } }
    // 첫번째 준결승때 이긴 챔피언의 인덱스
    private int _firstWinnerIndex;
    // 두번째 준결승때 이긴 챔피언의 인덱스 
    private int _secondWinnerIndex;

    // RPC로 보내줄 맴버들의 
    private int _firstMember;
    private int _secondMember;
    private int _thirdMember;
    private int _fourthMember;

    // 무승부인지 확인하는 변수 -> 현재 미사용
    private bool _isDraw;
    public bool IsDraw { get { return _isDraw; } private set { _isDraw = value; } }

    // 그룹내 챔피언의 순서를 랜덤하게 담을 값
    private int _randomIndex;

    private void OnEnable()
    {
        _setFirstBattle.RemoveListener(SetPositionFirstBattle);
        _setFirstBattle.AddListener(SetPositionFirstBattle);

        _FirstBattleWinnerSetFinalGroup.RemoveListener(FirstWinnerSetFinalGroup);
        _FirstBattleWinnerSetFinalGroup.AddListener(FirstWinnerSetFinalGroup);

        _SecondBattleWinnerSetFinalGroup.RemoveListener(SecondWinnerSetFinalGroup);
        _SecondBattleWinnerSetFinalGroup.AddListener(SecondWinnerSetFinalGroup);

        if (PhotonNetwork.IsMasterClient)
        {
            // 1번
            SettingRandomGroup();
            // 첫번째 준결승전 시작
            _setFirstBattle.Invoke();
        }
    }

    private void Update()
    {
        // 첫번째 전투 종료시
        if ((_member[0].activeSelf == false || _member[1].activeSelf == false) && !_isFirstBattle)
        {
            _FirstBattleWinnerSetFinalGroup.Invoke();

            _isFirstBattle = true;
        }

        // 두번째 전투 종료 시
        if ((_member[2].activeSelf == false || _member[3].activeSelf == false) && _member[0].activeSelf == false && _member[1].activeSelf == false && _isSecondBattle)
        {
            _SecondBattleWinnerSetFinalGroup.Invoke();

            _isSecondBattle = false;
        }

        // 경기 종료 및 우승자 인덱스 넣어줌
        if (_finalBattle[0] != null && _finalBattle[1] != null)
        {
            if ((_finalBattle[0].activeSelf == false || _finalBattle[1].activeSelf == false) && _isFinelBattle)
            {
                Invoke("Finish", 5f);

                _isFinelBattle = false;
            }
        }

    }

    

    /// <summary>
    /// 그룹 내 참가자들의 순서 정하기
    /// </summary>
    private void SettingRandomGroup()
    {
        _memberIndexList = new List<int>();

        for (int i = 0; i < _member.Length; ++i)
        {
            _randomIndex = Random.Range(0, 4);

            for (int j = 0; j < _memberIndexList.Count;)
            {
                if (_memberIndexList[j] == _randomIndex)
                {
                    j = 0;
                    _randomIndex = Random.Range(0, 4);
                }
                else
                {
                    ++j;
                }
            }
            _memberIndexList.Add(_randomIndex);
        }

        for (int i = 0; i < _member.Length; ++i)
        {
            int index;
            index = _memberIndexList[i];
            _member[i] = transform.GetChild(index).gameObject;
        }

        // 마스터가 짠 _member 그룹의 내용을 Others 에게 전달
        photonView.RPC("ClientsSettingGroup", RpcTarget.Others, _memberIndexList[0], _memberIndexList[1], _memberIndexList[2], _memberIndexList[3]);
    }

    /// <summary>
    /// SettingRandomGroup 클라버전
    /// </summary>
    [PunRPC]
    public void ClientsSettingGroup(int _memberIndexList_0, int _memberIndexList_1, int _memberIndexList_2, int _memberIndexList_3)
    {
        _firstMember = _memberIndexList_0;
        _secondMember = _memberIndexList_1;
        _thirdMember = _memberIndexList_2;
        _fourthMember = _memberIndexList_3;

        _member[0] = transform.GetChild(_firstMember).gameObject;
        _member[1] = transform.GetChild(_secondMember).gameObject;
        _member[2] = transform.GetChild(_thirdMember).gameObject;
        _member[3] = transform.GetChild(_fourthMember).gameObject;
        // 첫번째 준결승전 시작
        _setFirstBattle.Invoke();
    }

    /// <summary>
    /// 첫번째 전투 참가자들 위치 배치 및 생성
    /// </summary>
    private void SetPositionFirstBattle()
    {
        _member[0].transform.position = new Vector3(-_setPositionX, -4.5f, _setPositionZ);
        _member[0].transform.rotation = Quaternion.Euler(0, 90, 0);
        _member[1].transform.position = new Vector3(_setPositionX, -4.5f, _setPositionZ);
        _member[1].transform.rotation = Quaternion.Euler(0, -90, 0);

        _member[0].SetActive(true);
        _member[1].SetActive(true);
    }


    /// <summary>
    /// 두번재 전투 참가자들 위치 배치 및 생성
    /// </summary>
    private void SetPositionSecondBattle()
    {
        _member[2].transform.position = new Vector3(-_setPositionX, -4.5f, _setPositionZ);
        _member[2].transform.rotation = Quaternion.Euler(0, 90, 0);
        _member[3].transform.position = new Vector3(_setPositionX, -4.5f, _setPositionZ);
        _member[3].transform.rotation = Quaternion.Euler(0, -90, 0);

        _member[2].SetActive(true);
        _member[3].SetActive(true);

        _isSecondBattle = true;
    }

    /// <summary>
    /// 마지막 전투 참가자틀 위치 배치 및 생성
    /// </summary>
    private void SetPositionFinalBattle()
    {

        _finalBattle[0].transform.rotation = Quaternion.Euler(0, 90, 0);
        _finalBattle[1].transform.rotation = Quaternion.Euler(0, -90, 0);

        _finalBattle[0].SetActive(true);
        _finalBattle[1].SetActive(true);

        _isFinelBattle = true;
    }

    /// <summary>
    /// 첫번째 전투 승리를 최종 전투 참가자 그룹에 추가 및 두번째 전투 진행
    /// </summary>
    private void FirstWinnerSetFinalGroup()
    {
        if (_member[0].activeSelf)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _firstWinnerIndex = _memberIndexList[0];
            }
            else
            {
                _firstWinnerIndex = _firstMember;
            }

            _finalBattle[0] = _member[0];
            _member[0].transform.position = new Vector3(-_setPositionX, -4.5f, _setPositionZ);
            _member[0].SetActive(false);
        }

        else if (_member[1].activeSelf)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _firstWinnerIndex = _memberIndexList[1];
            }
            else
            {
                _firstWinnerIndex = _secondMember;
            }

            _finalBattle[0] = _member[1];
            _member[1].transform.position = new Vector3(-_setPositionX, -4.5f, _setPositionZ);
            _member[1].SetActive(false);
        }

        // 두번재 전투 위치 배치 및 생성
        Invoke("SetPositionSecondBattle", 2f);
    }

    /// <summary>
    /// 두번째 전투 승리를 최종 전투 참가자 그룹에 추가 및 마지막 전투 진행
    /// </summary>
    private void SecondWinnerSetFinalGroup()
    {
        if (_member[2].activeSelf)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _secondWinnerIndex = _memberIndexList[2];
            }
            else
            {
                _secondWinnerIndex = _thirdMember;
            }

            _finalBattle[1] = _member[2];
            _member[2].transform.position = new Vector3(_setPositionX, -4.5f, _setPositionZ);
            _member[2].SetActive(false);
        }

        else if (_member[3].activeSelf)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _secondWinnerIndex = _memberIndexList[3];
            }
            else
            {
                _secondWinnerIndex = _fourthMember;            
            }

            _finalBattle[1] = _member[3];
            _member[3].transform.position = new Vector3(_setPositionX, -4.5f, _setPositionZ);
            _member[3].SetActive(false);
        }

        Invoke("SetPositionFinalBattle", 2f);
    }

    /// <summary>
    /// 우승자 인덱스 넣어주기
    /// </summary>
    private void SendWinnerIndex()
    {
        if (_finalBattle[0].activeSelf)
        {
            _winnerIndex = _firstWinnerIndex;
        }
        else if (_finalBattle[1].activeSelf)
        {
            _winnerIndex = _secondWinnerIndex;
        }
    }

    /// <summary>
    /// 경기 종료 시 셋팅 초기화
    /// </summary>
    private void SettingReset()
    {
        _member[0].transform.position = Vector3.zero;
        _member[1].transform.position = Vector3.zero;
        _member[2].transform.position = Vector3.zero;
        _member[3].transform.position = Vector3.zero;

        _isFirstBattle = false;
        _isSecondBattle = false;
        _isFinelBattle = false;
        _isDraw = false;
        _firstWinnerIndex = 0;
        _secondWinnerIndex = 0;
        //_winnerIndex = 0;
        _randomIndex = 0;

        _member = new GameObject[4];
        _finalBattle = new GameObject[2];

        _setFirstBattle.RemoveListener(SetPositionFirstBattle);
        _FirstBattleWinnerSetFinalGroup.RemoveListener(FirstWinnerSetFinalGroup);
        _SecondBattleWinnerSetFinalGroup.RemoveListener(SecondWinnerSetFinalGroup);
    }

    /// <summary>
    /// 우승자 나올 시 경기 끝내는 함수
    /// </summary>
    private void Finish()
    {
        SendWinnerIndex();

        _finalBattle[0].SetActive(false);
        _finalBattle[1].SetActive(false);

        _finishTournament.Invoke();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SettingReset();
    }
}
