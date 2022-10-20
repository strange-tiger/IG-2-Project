using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EFoodSatietyLevel
{
    None,
    Small,
    Big
};



public class Food : MonoBehaviour
{
    public static UnityEvent<EFoodSatietyLevel> OnEated = new UnityEvent<EFoodSatietyLevel>();

    [SerializeField] EFoodSatietyLevel _foodSatietyLevel;
    [SerializeField] GameObject _food;
    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);


    public void Eated()
    {
        OnEated.Invoke(_foodSatietyLevel);

        _food.SetActive(false);

        StartCoroutine(RegenerateFood());
    }

    IEnumerator RegenerateFood()
    {
        yield return _waitSecondRegenerate;

        _food.SetActive(true);

        yield return null;
    }


}
