using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDamageNum : MonoBehaviour
{
    [SerializeField]
    private int _damage;

    public int Damage
    {
        get
        {
            return _damage;
        }

        set
        {
            _damage = value;
        }
    }

    void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
