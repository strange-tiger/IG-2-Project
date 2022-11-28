using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireQuest : QuestConducter
{
    [SerializeField] private GameObject _props;

    public override void StartQuest()
    {
        _props.SetActive(true);
    }

    private void OnDisable()
    {
        _props.SetActive(false);
    }
}
