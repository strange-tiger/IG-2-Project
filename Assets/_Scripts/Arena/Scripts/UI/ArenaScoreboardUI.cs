using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ArenaScoreboardUI : MonoBehaviour, IPunObservable
{
    [SerializeField] private GroupManager _groupManager;

    [Header("참가자들 텍스트")]
    [SerializeField] private TextMeshProUGUI[] _championNameText;

    [Header("참가자들 Hp 슬라이드")]
    [SerializeField] private Slider[] _championSlider;

    [Header("얻어올 참가자들 Hp")]
    [SerializeField] private AIDamage[] _championHp;

    [Header("타이머 텍스트")]
    [SerializeField] private TextMeshProUGUI _timeText;

    private float[] _hp = new float[4];

    private int _minute = 15;
    private int _second = 0;
    private float _cumulativeTime;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_minute);
            stream.SendNext(_second);
        }
        else
        {
            _minute = (int)stream.ReceiveNext();
            _second = (int)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        SetChampionInfo();
        // photonView.RPC("SetChampionInfo", RpcTarget.All);
    }

    private void Update()
    {
        FlowingTime();
        UpdateTimerText(_minute, _second);
       // photonView.RPC("UpdateTimerText", RpcTarget.All, _minute, _second);
        
        SetChampionHp();
       // photonView.RPC("SetChampionHp", RpcTarget.All);
    }

    /// <summary>
    /// 타이머
    /// </summary>
    private void FlowingTime()
    {
        //if (PhotonNetwork.IsMasterClient)
        {
            _cumulativeTime += Time.deltaTime;

            if (_cumulativeTime > 1f)
            {
                if (_second == 0)
                {
                    _second = 59;
                    --_minute;
                }
                else
                {
                    --_second;
                }
                _cumulativeTime = 0f;
            }

            if (_minute < 0)
            {
                _minute = 0;
                _second = 0;

                Time.timeScale = 0;
            }
        }
    }

    /// <summary>
    /// 타이머 텍스트 출력
    /// </summary>
    /// <param name="minute">분</param>
    /// <param name="second">초</param>
    [PunRPC]
    public void UpdateTimerText(int minute, int second)
    {
        _timeText.text = $"{minute} : {second:D2}";
    }

    [PunRPC]
    public void SetChampionHp()
    {
        for (int i = 0; i < _championSlider.Length; ++i)
        {
            _championSlider[i].value = _championHp[i].Hp / _hp[i] * 100;
        }
    }

    [PunRPC]
    public void SetChampionInfo()
    {
        for (int i = 0; i < _championNameText.Length; ++i)
        {
            _championNameText[i].text = _groupManager.transform.GetChild(i).name;
        }

        for (int i = 0; i < _championSlider.Length; ++i)
        {
            _hp[i] = _championHp[i].Hp;
        }
    }
}
