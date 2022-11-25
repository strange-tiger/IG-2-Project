using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempQuest : QuestConducter
{
    public override void StartQuest()
    {
        Debug.Log("[Tutorial] Quest ½ÇÇàµÊ");
        StopAllCoroutines();
        StartCoroutine(EndQuest());
    }

    private IEnumerator EndQuest()
    {
        yield return new WaitForSeconds(1f);
        QuestEnded();
    }
}
