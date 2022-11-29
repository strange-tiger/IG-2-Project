using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestConducter : MonoBehaviour
{
    public delegate void QuestEnd();
    public event QuestEnd OnQuestEnd;


    protected virtual void OnEnable()
    {
        OnQuestEnd -= OnQuestEnded;
        OnQuestEnd += OnQuestEnded;
        StartQuest();
    }

    public abstract void StartQuest();

    protected virtual void OnQuestEnded()
    {
        Debug.Log("[Tutorial] Quest ³¡³²");
    }

    protected void QuestEnded()
    {
        OnQuestEnd.Invoke();
    }
}
