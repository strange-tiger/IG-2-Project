using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstHitQuest : QuestConducter
{
    [SerializeField] private GameObject _mapNPC;
    [SerializeField] private GameObject _props;
    private FirstMoveAttackNPCForTurorial _npc;

    private void Awake()
    {
        _npc = _props.GetComponentInChildren<FirstMoveAttackNPCForTurorial>();
        _npc.OnNPCHit -= QuestEnded;
        _npc.OnNPCHit += QuestEnded;
    }

    public override void StartQuest()
    {
        _mapNPC.SetActive(false);
        _props.SetActive(true);
    }

    private void OnDisable()
    {
        _props.SetActive(false);
        _mapNPC.SetActive(true);
    }
}
