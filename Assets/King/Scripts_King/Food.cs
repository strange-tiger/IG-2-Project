using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Food : MonoBehaviour
{
    [SerializeField] bool _isBigFood;
    [SerializeField] GameObject _food;
    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);
    public static int SatietyStack;

    private void Start()
    {
    }


    public void Eated()
    {
        if(_isBigFood)
        {
            SatietyStack += 2;
        }
        else
        {
            SatietyStack++;
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
