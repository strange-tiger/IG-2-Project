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
        // �ܼ��� �÷��̾ ���Դ����� Ȯ���ϰ� UI �� �غ�
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
        // ���� ������ �����س��� ��
        float stop = 5;
        if (randNum <= stop && stop <= (randNum + 10))
        {
            // �����̵� ������ �߰�
        }

    }

}
