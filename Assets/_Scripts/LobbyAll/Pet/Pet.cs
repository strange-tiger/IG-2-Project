using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : InteracterableObject
{

    [SerializeField] EPetEvolutionCount _petEvolutionCount;
    public EPetEvolutionCount PetEvolutionCount { get { return _petEvolutionCount; } }
    public override void Interact()
    {
        base.Interact();

    }


}


public enum EPetEvolutionCount
{
    NONE,
    ZERO,
    ONE,
    TWO,
};