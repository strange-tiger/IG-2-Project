using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [Header("��� ������ �������� �����ּ���")]
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
