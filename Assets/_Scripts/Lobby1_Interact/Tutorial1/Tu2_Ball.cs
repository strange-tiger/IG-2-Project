using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu2_Ball : MonoBehaviour
{
    [SerializeField] private Tu2_BallGame _tu2_BallGame;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Ball"))
        {
            ++_tu2_BallGame.AdvanceQuest;

            enabled = false;
        }
    }
}
