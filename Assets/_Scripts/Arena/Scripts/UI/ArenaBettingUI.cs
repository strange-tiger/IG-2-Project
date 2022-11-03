using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArenaBettingUI : MonoBehaviour
{
    
    [SerializeField] private TournamentManager _tournamentManager;
    
    [SerializeField] private TextMeshProUGUI _groupNameText;

    [Header("참가자들 텍스트")]
    [SerializeField] private TextMeshProUGUI[] _championNameText;

    void Start()
    {
        _groupNameText.text = _tournamentManager.Groups[_tournamentManager.SelectGroup].name;

        for (int i = 0; i < 4; ++i)
        {
            _championNameText[i].text = _tournamentManager.Groups[_tournamentManager.SelectGroup].transform.GetChild(i).name;
        }
    }
}
