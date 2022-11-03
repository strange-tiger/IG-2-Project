using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArenaBettingUI : MonoBehaviour
{
    [Header("�׷��� �־��ּ���")]
    [SerializeField] private GameObject _group;

    [Header("�׷� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI _groupNameText;

    [Header("�����ڵ�")]
    [SerializeField] private GameObject[] _champion;

    [Header("�����ڵ� �ؽ�Ʈ")]
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
