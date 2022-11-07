using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{

    [SerializeField] PetData _petData;
    [SerializeField] GameObject _petObject;
    private int _ranNum;

    void Start()
    {
        _ranNum = Random.Range(0, 16);

        _petObject = Instantiate(_petData.PetObject[_ranNum]);

        _petObject.transform.GetChild(_petData.PetAsset[_ranNum]).gameObject.SetActive(true);
           
    }


    void Update()
    {

    }
}
