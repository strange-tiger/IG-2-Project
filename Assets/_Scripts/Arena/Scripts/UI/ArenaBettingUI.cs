using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArenaBettingUI : MonoBehaviour
{
    [Header("그룹을 넣어주세요")]
    [SerializeField] private GameObject _group;

    [Header("그룹 텍스트")]
    [SerializeField] private TextMeshProUGUI _groupNameText;

    [Header("참가자들")]
    [SerializeField] private GameObject[] _champion;

    [Header("참가자들 텍스트")]
    [SerializeField] private TextMeshProUGUI[] _championNameText;



    void Start()
    {
        _groupNameText.text = _group.name;

        for (int i = 0; i < _champion.Length; ++i) 
        {
            _championNameText[i].text = _champion[i].name;
        }
    }

    void Update()
    {
        
    }
}
