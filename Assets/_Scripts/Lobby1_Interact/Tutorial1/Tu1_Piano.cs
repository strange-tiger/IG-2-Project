using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu1_Piano : MonoBehaviour
{
    [SerializeField] private Tu1_PerfectPitch _tu1_PerfectPitch;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("PlayerBody"))
        {
            ++_tu1_PerfectPitch.AdvanceQuest;

            enabled = false;
        }
    }
}
