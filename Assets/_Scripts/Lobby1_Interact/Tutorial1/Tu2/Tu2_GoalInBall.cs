using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tu2_GoalInBall : MonoBehaviour
{
    private ParticleSystem[] _goalIn;

    private void Awake()
    {
        _goalIn = new ParticleSystem[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            _goalIn[i] = gameObject.transform.GetChild(i).GetComponent<ParticleSystem>();
            _goalIn[i].gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider _goalLine)
    {
            if (_goalLine.gameObject.tag == "Ball" && _goalLine.gameObject.transform.position.y > gameObject.transform.position.y)
            {
                PlayGoalInParticle();
            }
    }

    private void PlayGoalInParticle()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            _goalIn[i].gameObject.SetActive(true);
        }
    }

}
