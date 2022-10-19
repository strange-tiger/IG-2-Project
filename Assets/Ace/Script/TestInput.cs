using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    private Vector3 curDir;
    [SerializeField]
    private float _speed = 0.5f;

    private void Awake()
    {
        curDir = Vector3.zero;
    }

    void Update()
    {
        Move();
    }
    private void Move()
    {

        curDir = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            curDir.x = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            curDir.x = 1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            curDir.z = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            curDir.z = -1;
        }

        curDir.Normalize();

        transform.position += curDir * (_speed * Time.deltaTime);
    }
}