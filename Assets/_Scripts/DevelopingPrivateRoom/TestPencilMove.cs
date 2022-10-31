using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPencilMove : MonoBehaviour
{
    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.3f));
    }
}
