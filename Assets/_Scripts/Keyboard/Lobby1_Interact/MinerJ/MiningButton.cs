using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _slider;
    [SerializeField]
    private GameObject _circleCheckBox;
    
    public void OnClickButton()
    {
        gameObject.SetActive(false);
        _slider.SetActive(true);
        _circleCheckBox.SetActive(true);
    }
}
