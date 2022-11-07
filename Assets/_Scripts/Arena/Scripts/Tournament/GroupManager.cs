using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class GroupManager : MonoBehaviourPun
{
    [SerializeField] private int _setPosition;

    private GameObject[] _member = new GameObject[4];
    public GameObject[] Member { get { return _member; } }

    private GameObject[] _finalBattle = new GameObject[2];

    // �ذ�� 1 ��ġ ���� �� �ִ� �̺�Ʈ
    private UnityEvent _setFirstBattle = new UnityEvent();
    private UnityEvent _FirstBattleWinnerSetFinalGroup = new UnityEvent();
    private UnityEvent _SecondBattleWinnerSetFinalGroup = new UnityEvent();

    List<int> _memberIndexList;

    private bool _isFirstBattle;
    private bool _isSecondBattle;
    private bool _isFinelBattle;

    private int _winnerIndex;
    private int _firstWinnerIndex;
    private int _secondWinnerIndex;
    public int WinnerIndex { get { return _winnerIndex; } private set { _winnerIndex = value; } }

    private bool _isDraw;
    public bool IsDraw { get { return _isDraw; } private set { _isDraw = value; } }

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
            SettingRandomGroup();
        }

        _setFirstBattle.Invoke();
    }

    private void Update()
    {
        // ù��° ���� �����
        if ((_member[0].activeSelf == false || _member[1].activeSelf == false) && !_isFirstBattle)
        {
            _FirstBattleWinnerSetFinalGroup.Invoke();
            //photonView.RPC("FirstWinnerSetFinalGroup", RpcTarget.Others);

            _isFirstBattle = true;
        }

        // �ι�° ���� ���� ��
        if ((_member[2].activeSelf == false || _member[3].activeSelf == false) && _member[0].activeSelf == false && _member[1].activeSelf == false && _isSecondBattle)
        {
            _SecondBattleWinnerSetFinalGroup.Invoke();
            //photonView.RPC("SecondWinnerSetFinalGroup", RpcTarget.Others);

            _isSecondBattle = false;
        }

        // ��� ���� �� ����� �ε��� �־���
        if (_finalBattle[0] != null && _finalBattle[1] != null)
        {
            if ((_finalBattle[0].activeSelf == false || _finalBattle[1].activeSelf == false) && _isFinelBattle)
            {
                SendWinnerIndex();
                //photonView.RPC("SendWinnerIndex", RpcTarget.Others);

                Debug.Log(WinnerIndex);

                Invoke("Finish", 15f);

                _isFinelBattle = false;
            }
        }

    }

    private void OnDisable()
    {
        SettingReset();
    }

    /// <summary>
    /// �׷� �� �����ڵ��� ���� ���ϱ�
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

        Debug.Log($"{_memberIndexList[0]}, {_memberIndexList[1]}, {_memberIndexList[2]}, {_memberIndexList[3]}");

        for (int i = 0; i < _member.Length; ++i)
        {
            int index;
            index = _memberIndexList[i];
            _member[i] = transform.GetChild(index).gameObject;
        }

        // ù��° �������� �̺�Ʈ

        //photonView.RPC("SetPositionFirstBattle", RpcTarget.Others);
    }

    /// <summary>
    /// ù��° ���� �����ڵ� ��ġ ��ġ �� ����
    /// </summary>
    //[PunRPC]
    public void SetPositionFirstBattle()
    {
        _member[0].transform.position = new Vector3(-_setPosition, -2f, 0);
        _member[0].transform.rotation = Quaternion.Euler(0, 90, 0);
        _member[1].transform.position = new Vector3(_setPosition, -2f, 0);
        _member[1].transform.rotation = Quaternion.Euler(0, -90, 0);

        _member[0].SetActive(true);
        _member[1].SetActive(true);
    }


    /// <summary>
    /// �ι��� ���� �����ڵ� ��ġ ��ġ �� ����
    /// </summary>
    //[PunRPC]
    public void SetPositionSecondBattle()
    {
        _member[2].transform.position = new Vector3(-_setPosition, -2f, 0);
        _member[2].transform.rotation = Quaternion.Euler(0, 90, 0);
        _member[3].transform.position = new Vector3(_setPosition, -2f, 0);
        _member[3].transform.rotation = Quaternion.Euler(0, -90, 0);

        _member[2].SetActive(true);
        _member[3].SetActive(true);

        _isSecondBattle = true;
    }

    /// <summary>
    /// ������ ���� ������Ʋ ��ġ ��ġ �� ����
    /// </summary>
    //[PunRPC]
    public void SetPositionFinalBattle()
    {

        _finalBattle[0].transform.rotation = Quaternion.Euler(0, 90, 0);
        _finalBattle[1].transform.rotation = Quaternion.Euler(0, -90, 0);

        _finalBattle[0].SetActive(true);
        _finalBattle[1].SetActive(true);

        _isFinelBattle = true;
    }

    /// <summary>
    /// ù��° ���� �¸��� ���� ���� ������ �׷쿡 �߰� �� �ι�° ���� ����
    /// </summary>
    //[PunRPC]
    public void FirstWinnerSetFinalGroup()
    {
        if (_member[0].activeSelf)
        {
            _firstWinnerIndex = _memberIndexList[0];
            _finalBattle[0] = _member[0];
            _member[0].transform.position = new Vector3(-_setPosition, -2f, 0);
            _member[0].SetActive(false);
        }

        else if (_member[1].activeSelf)
        {
            _firstWinnerIndex = _memberIndexList[1];
            _finalBattle[0] = _member[1];
            _member[1].transform.position = new Vector3(-_setPosition, -2f, 0);
            _member[1].SetActive(false);
        }

        // �ι��� ���� ��ġ ��ġ �� ����
        Invoke("SetPositionSecondBattle", 2f);
        //photonView.RPC("SetPositionSecondBattle", RpcTarget.Others);
    }

    /// <summary>
    /// �ι�° ���� �¸��� ���� ���� ������ �׷쿡 �߰� �� ������ ���� ����
    /// </summary>
    //[PunRPC]
    public void SecondWinnerSetFinalGroup()
    {
        if (_member[2].activeSelf)
        {
            _secondWinnerIndex = _memberIndexList[2];
            _finalBattle[1] = _member[2];
            _member[2].transform.position = new Vector3(_setPosition, -2f, 0);
            _member[2].SetActive(false);
        }

        else if (_member[3].activeSelf)
        {
            _secondWinnerIndex = _memberIndexList[3];
            _finalBattle[1] = _member[3];
            _member[3].transform.position = new Vector3(_setPosition, -2f, 0);
            _member[3].SetActive(false);
        }

        Invoke("SetPositionFinalBattle", 2f);
        //photonView.RPC("SetPositionFinalBattle", RpcTarget.Others);
    }

    /// <summary>
    /// ����� �ε��� �־��ֱ�
    /// </summary>
    //[PunRPC]
    public void SendWinnerIndex()
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
    /// ��� ���� �� ���� �ʱ�ȭ
    /// </summary>
    //[PunRPC]
    public void SettingReset()
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
        _winnerIndex = 0;
        _randomIndex = 0;

        _member = new GameObject[4];
        _finalBattle = new GameObject[2];

        _setFirstBattle.RemoveListener(SetPositionFirstBattle);
        _FirstBattleWinnerSetFinalGroup.RemoveListener(FirstWinnerSetFinalGroup);
        _SecondBattleWinnerSetFinalGroup.RemoveListener(SecondWinnerSetFinalGroup);
    }

    /// <summary>
    /// ��
    /// </summary>

    private void Finish()
    {
        _finalBattle[_winnerIndex].SetActive(false);
        gameObject.SetActive(false);

        //photonView.RPC("SendFinish", RpcTarget.Others, _winnerIndex);
    }

    [PunRPC]
    public void SendFinish(int num)
    {
        _finalBattle[num].SetActive(false);
        gameObject.SetActive(false);
    }
}
