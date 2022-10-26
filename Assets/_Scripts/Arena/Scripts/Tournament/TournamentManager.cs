using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _groups;

    private int _selectGroup;
    private int _finalWinnerIndex;
    
    public int FinalWinnerIndex
    {
        get
        {
            return _finalWinnerIndex;
        }
        private set
        {
            _finalWinnerIndex = value;
        }
    }

    private void OnEnable()
    {
        _selectGroup = Random.Range(0, _groups.Length);

        _groups[_selectGroup].SetActive(true);
    }

    private void OnDisable()
    {
        _groups[_selectGroup].SetActive(false);
        _selectGroup -= _selectGroup;
    }
}
