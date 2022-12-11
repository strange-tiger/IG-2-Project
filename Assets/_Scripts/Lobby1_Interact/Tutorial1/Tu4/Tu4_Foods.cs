using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EFoodSatietyLevels
{
    None,
    Small,
    Big
};

public class Tu4_Foods : InteracterableObject
{
    public static UnityEvent<EFoodSatietyLevels> OnEatedTu = new UnityEvent<EFoodSatietyLevels>();

    [SerializeField] EFoodSatietyLevels _foodSatietyLevel;
    [SerializeField] GameObject _food;
    [SerializeField] Collider _collider;
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private FoodInteraction _foodInteraction;
    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);


    public override void Interact()
    {
        _foodInteraction = FindObjectOfType<PlayerInteraction>().transform.root.GetComponent<FoodInteraction>();

        if (_foodInteraction.SatietyStack != 6)
        {

            OnEatedTu.Invoke(_foodSatietyLevel);

            EatFoodState();

            StartCoroutine(RegenerateFood());

        }

        ++_lobby1TutorialsQuest.AdvanceQuest;

        if (_lobby1TutorialsQuest.AdvanceQuest >= 5)
        {
            _lobby1TutorialsQuest.AdvanceQuest = 5;
        }
    }

    private IEnumerator RegenerateFood()
    {
        yield return _waitSecondRegenerate;

        RegenerateFoodState();

        yield return null;
    }

    private void EatFoodState()
    {
        _food.SetActive(false);
        _collider.enabled = false;
    }

    private void RegenerateFoodState()
    {
        _food.SetActive(true);
        _collider.enabled = true;
    }
}
