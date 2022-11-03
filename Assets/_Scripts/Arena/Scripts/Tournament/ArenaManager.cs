using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [Header("몇분 단위로 시작할지 적어주세요")]
    [SerializeField] private int _startMinute;

    [SerializeField] private GameObject _tournamentManager;

    private int _startSecond;

    void Start()
    {
        _startSecond *= _startMinute * 60;

        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        while (true)
        {
            yield return new WaitForSeconds((float)_startSecond);

            _tournamentManager.SetActive(true);
        }
    }
}
