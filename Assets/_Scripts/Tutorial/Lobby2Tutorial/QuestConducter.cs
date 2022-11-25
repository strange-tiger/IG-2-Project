using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestConducter : MonoBehaviour
{
    public delegate void QuestEnd();
    public event QuestEnd OnQuestEnd;


    protected virtual void OnEnable()
    {
        StartQuest();
    }

    public abstract void StartQuest();

    protected void QuestEnded()
    {
        OnQuestEnd.Invoke();
    }
}
