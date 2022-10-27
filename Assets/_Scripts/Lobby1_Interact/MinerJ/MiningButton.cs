using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _slider;
    
    public void OnClickButton()
    {
        gameObject.SetActive(false);
        _slider.SetActive(true);
    }
}
