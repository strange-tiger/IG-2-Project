using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetInteract : InteracterableObject
{

    [SerializeField] EPetEvolutionCount _petEvolutionCount;
    public EPetEvolutionCount PetEvolutionCount { get { return _petEvolutionCount; } }
    public override void Interact()
    {
    }
}
