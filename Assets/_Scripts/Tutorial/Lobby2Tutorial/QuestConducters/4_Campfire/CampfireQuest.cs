using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireQuest : QuestConducter
{
    [SerializeField] private GameObject _props;
    private WoodPileForTutorial _woodPile;

    [SerializeField] private int _woodCountForQuestEnd = 3;

    [SerializeField] private int _woodCount = 0;

    private void Awake()
    {
        _woodPile = _props.GetComponentInChildren<WoodPileForTutorial>();
    }

    public override void StartQuest()
    {
        _woodPile.ResetWood();
        _woodCount = 0;
        _props.SetActive(true);

        base.StartQuest();
    }

    public void StackWood()
    {
        ++_woodCount;

        if(_woodCount >= _woodCountForQuestEnd)
        {
            QuestEnded();
        }
    }

    public void WoodOut()
    {
        _woodCount = _woodCount > 0 ? _woodCount - 1 : 0;
    }

    private void OnDisable()
    {
        _props.SetActive(false);
    }
}
