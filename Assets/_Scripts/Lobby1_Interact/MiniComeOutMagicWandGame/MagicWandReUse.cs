using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class MagicWandReUse : MonoBehaviourPun
{
    [Header("ÄðÅ¸ÀÓÀ» °ñ¶óÁÖ¼¼¿ä")]
    [SerializeField] private Defines.CoolTime _coolTime;

    [SerializeField] private MagicWand _magicWand;
    [SerializeField] private GameObject _magicWandPanel;

    private TextMeshProUGUI _magicCoolTimeText;
    private float _currentTime;
    private int _coolTimeText;

    void Start()
    {
        
    }

    void Update()
    {
        if (_magicWand.CheckCoolTime)
        {
            _currentTime += Time.deltaTime;

            _coolTimeText = (int)_coolTime;

            _coolTimeText -= (int)_currentTime;
            _magicCoolTimeText.text = _coolTimeText.ToString();

            if (_currentTime > (float)_coolTime)
            {
                _currentTime -= _currentTime;
                _magicWand.CheckCoolTime = false;
                _magicWandPanel.SetActive(false);
            }
        }
    }
}
