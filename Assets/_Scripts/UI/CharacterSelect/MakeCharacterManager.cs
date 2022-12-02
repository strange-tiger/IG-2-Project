using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Asset.MySql;
using UnityEngine.SceneManagement;
using Photon.Pun;

using SceneType = Defines.ESceneNumber;

public class MakeCharacterManager : MonoBehaviourPun
{
    // 플레이어 조작 튜토리얼에서 사용되는 여성 버튼을 눌렀을 때 호출 할 이벤트
    public UnityEvent OnClickFemaleButton = new UnityEvent();

    // 플레이어에게 커스터마이즈를 적용시켜 주는 PlayerCustomize
    [Header("Player")]
    [SerializeField] private PlayerCustomize _playerCustomize;

    // 성별 선택과 캐릭터를 만드는 버튼
    [Header("UI")]
    [SerializeField] private Button _maleSelectButton;
    [SerializeField] private Button _femaleSelectButton;
    [SerializeField] private Button _makeCharacterButton;

    // 한 성별을 선택했을 때, 그 성별 버튼의 중복 선택을 막기위한 패널과 튜토리얼 때 아웃라인을 보여줄 패널
    [Header("Block Raycast")]
    [SerializeField] private GameObject _femalePanel;
    [SerializeField] private GameObject _malePanel;
    [SerializeField] private GameObject _rayPlane;

    private void OnEnable()
    {
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _maleSelectButton.onClick.AddListener(SelectMale);

        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _femaleSelectButton.onClick.AddListener(SelectFemale);

        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);
        _makeCharacterButton.onClick.AddListener(CreateCharacter);

        Application.wantsToQuit -= PlayerOffline;
        Application.wantsToQuit += PlayerOffline;

        // 초기 캐릭터 성별은 남성이므로 여성의 선택을 가능하게 함
        _femalePanel.SetActive(false);
    }



    // 남성 버튼을 선택했을 때
    private void SelectMale()
    {
        // PlayerCustomize에서 성별을 남성으로 바꾼 후
        _playerCustomize.IsFemale = false;
        // 플레이어 모델을 초기화
        _playerCustomize.MakeAvatarData();
        // 남성버튼은 선택할 수 없게 하고 
        _malePanel.SetActive(true);
        // 여성버튼은 선택이 가능 하도록 한다.
        _femalePanel.SetActive(false);
    }

    // 여성 버튼을 선택했을 때
    private void SelectFemale()
    {
        // 플레이어 조작 튜토리얼 매니저에게 여성버튼을 클릭했다는 이벤트를 호출함.
        OnClickFemaleButton.Invoke();
        _playerCustomize.IsFemale = true;
        _playerCustomize.MakeAvatarData();
        _malePanel.SetActive(false);
        _femalePanel.SetActive(true);
        _rayPlane.SetActive(false);
    }

    // 캐릭터 생성 버튼을 눌렀을 때
    private void CreateCharacter()
    {
        // 계정의 캐릭터 생성 여부를 업데이트
        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.HaveCharacter, "1");

        // 플레이어의 캐릭터 DB를 생성
        MySqlSetting.AddNewCharacter(TempAccountDB.Nickname, $"{Convert.ToInt32(_playerCustomize.IsFemale)}");

        // 펫 인벤토리 DB 생성
        MySqlSetting.AddNewPetInventory(TempAccountDB.Nickname);
        
        // 시작의 방으로 넘어감
        SceneManager.LoadScene((int)SceneType.StartRoom);
    }

    // 캐릭터 생성씬에는 LobbyManager가 없으므로 게임을 종료할 때, 계정의 Online 여부를 업데이트 해주어야함.
    private bool PlayerOffline()
    {
        try
        {
            Debug.Log("[Player] Offline Update");

            MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 0);

            return true;
        }
        catch (System.Exception error)
        {
            Debug.LogError(error);

            return false;
        }
    }
    private void OnDisable()
    {
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);
        Application.wantsToQuit -= PlayerOffline;
    }

    private void OnApplicationQuit()
    {
       
        Debug.Log("[Player] Offline Update");
        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 0);
    }
}
