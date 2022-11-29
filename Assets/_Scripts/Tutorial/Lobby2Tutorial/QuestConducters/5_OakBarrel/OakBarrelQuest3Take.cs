using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakBarrelQuest3Take : QuestConducter
{
    [SerializeField] private GameObject _mapNPC;
    [SerializeField] private GameObject _npc;
    private OakBarrelNPCForTurorial _oakBarrelNPC;

    private void Awake()
    {
        _oakBarrelNPC = _npc.GetComponentInChildren<OakBarrelNPCForTurorial>();
        _oakBarrelNPC.OnQuestEnd -= QuestEnded;
        _oakBarrelNPC.OnQuestEnd += QuestEnded;
    }

    public override void StartQuest()
    {
        _mapNPC.SetActive(false);

        _oakBarrelNPC.PrepareForQuest();
        _npc.SetActive(true);
    }

    private void OnDisable()
    {
        _npc.SetActive(false);
        _mapNPC.SetActive(true);
    }
}
