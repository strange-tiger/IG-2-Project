using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private float _cooldown = 5f;
    private void Awake()
    {

    }

    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        // 단순히 플레이어가 들어왔는지만 확인하고 UI 켤 준비
        if (collision.gameObject.tag == "Player")
        {

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    

        float randNum = Random.Range(0, 350);
        // 멈춘 각도를 저장해놓고 비교
        float stop = 5;
        if (randNum <= stop && stop <= (randNum + 10))
        {
            // 슬라이드 게이지 추가
        }

    }

}
