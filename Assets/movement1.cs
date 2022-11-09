using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object forward along its z axis 1 unit/second.
        transform.Translate(0, 0, 5 * Time.deltaTime);

       
    }
}
