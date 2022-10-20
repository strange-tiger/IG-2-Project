using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EFoodSatietyLevel
{
    None,
    Small,
    Big
};



public class Food : MonoBehaviour
{
    [SerializeField] bool _isBigFood;
    [SerializeField] GameObject _food;
    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);

    private void Start()
    {
    }


    public void Eated()
    {
        if(_isBigFood)
        {
            FoodInteraction.SatietyStack += (int)EFoodSatietyLevel.Big;
        }
        else
        {
            FoodInteraction.SatietyStack += (int)EFoodSatietyLevel.Small;
        }

        _food.SetActive(false);

        StartCoroutine(RegenerateFood());
    }

    IEnumerator RegenerateFood()
    {
        yield return _waitSecondRegenerate;

        _food.SetActive(true);

        yield return null;
    }

    private void OnDisable()
    {

    }
}
