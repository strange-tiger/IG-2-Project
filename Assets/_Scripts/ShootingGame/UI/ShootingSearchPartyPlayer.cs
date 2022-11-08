using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ShootingSearchPartyPlayer : MonoBehaviourPun
{
    [SerializeField] private GameObject _gunPrefab;
    [SerializeField] private Transform[] _handPositions = new Transform[2];

    public GameObject SearchPanel 
    { 
        get
        {
            return _searchPanel;
        }
        set
        {
            _searchPanel = value;
            _searchInfoText = _searchPanel.GetComponentInChildren<TextMeshProUGUI>();
            _cancelButton = _searchPanel.GetComponentInChildren<Button>();
        }
    }

    private GameObject _searchPanel;
    private TextMeshProUGUI _searchInfoText;
    private Button _cancelButton;

    private ShootingGameStartNPC _shootingGameStartScript;
    private PlayerInput _input;

    private bool _isSearching = false;
    private float _elapsedTime = 0f;

    public void ShowSearchUI(ShootingGameStartNPC shootingGameStartScript)
    {
        _input = transform.root.GetComponentInChildren<PlayerInput>();
        _shootingGameStartScript = shootingGameStartScript;

        _cancelButton.enabled = true;
        _cancelButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.AddListener(() =>
       {
           _shootingGameStartScript.CancelSearching(this);
       });

        _elapsedTime = 0f;
        _isSearching = true;
        _searchPanel.SetActive(true);
        StartCoroutine(CoSearchingForParty());
    }

    private IEnumerator CoSearchingForParty()
    {
        while(true)
        {
            _elapsedTime += Time.deltaTime;

            _searchInfoText.text = "Searching...\n" + GetStringTime(_elapsedTime);

            yield return null;
        }
    }

    private string GetStringTime(float elapsedTime)
    {
        int minute = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;

        string result = "";
        
        if(minute < 10)
        {
            result += "0";
        }
        result += minute.ToString() + ":";

        if(seconds < 10)
        {
            result += "0";
        }
        result += seconds.ToString();

        return result;
    }

    private void Update()
    {
        if(_isSearching)
        {
            if(OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four))
            {
                CancelSearching();
            }
        }
    }

    public void CancelSearching()
    {
        _shootingGameStartScript.CancelSearching(this);
        _isSearching = false;
        _searchPanel.SetActive(false);
    }

    public void FoundParty(ShootingGameManager shootingGameManager)
    {
        StopAllCoroutines();
        _searchInfoText.text = "Search Complete!\n" + GetStringTime(_elapsedTime);
        OVRScreenFade.instance.FadeOut();
        Invoke("PrepareForGame", 1.5f);
    }

    private void PrepareForGame(ShootingGameManager shootingGameManager)
    {
        GameObject gun = PhotonNetwork.Instantiate(_gunPrefab.name, transform.position, transform.rotation);
        gun.transform.parent = transform.root;
        gun.GetComponent<GunShoot>().photonView.RPC("Rest", RpcTarget.All, _handPositions, shootingGameManager);
    }
}
