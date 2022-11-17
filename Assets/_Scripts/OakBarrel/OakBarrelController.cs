using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakBarrelController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _oakBarrelList;

    void Start()
    {
        for (int i = 0; i < _oakBarrelList.Count; ++i)
        {
            _oakBarrelList.Add(transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        
    }
}
