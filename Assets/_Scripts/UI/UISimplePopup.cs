using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISimplePopup : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _list = new List<GameObject>();
    
    public void SetOn(bool value)
    {
        for(int i = 0; i < _list.Count; i++)
        {
            _list[i].SetActive(value);
        }
    }
}
