using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakBarrelController : MonoBehaviour
{
    private List<GameObject> _oakBarrelList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            _oakBarrelList.Add(transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        
    }
}
