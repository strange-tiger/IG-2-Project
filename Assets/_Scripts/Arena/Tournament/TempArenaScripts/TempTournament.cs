using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTournament : MonoBehaviour
{
    [Header("경기 진행시간")]
    [SerializeField] private float _playTime;

    [SerializeField] private GameObject[] _tournamentGrougs;
 
    private int _randomGroupNum;

    private void Awake()
    {
        _randomGroupNum = Random.Range(0, _tournamentGrougs.Length);

        _randomGroupNum = 0;

        _tournamentGrougs[_randomGroupNum].SetActive(true);
    }
}
