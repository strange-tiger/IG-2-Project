using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/PetData", fileName = "PetData")]
public class PetData : ScriptableObject
{
    [SerializeField] GameObject[] petObject;
    public GameObject[] PetObject { get { return petObject; } }

    [SerializeField] EPetStatus[] petStatus;
    public EPetStatus[] PetStatus { get { return petStatus; } }

    [SerializeField] int[] petLevel;
    public int[] PetLevel { get { return petLevel; } }

    [SerializeField] int[] petExp;
    public int[] PetExp { get { return petExp; } }

    [SerializeField] float[] petSize;
    public float[] PetSize { get { return petSize; } }

    [SerializeField] int[] petAsset;
    public int[] PetAsset { get { return petAsset; } }
}

public enum EPetStatus
{
    NONE,
    HAVE,
    EQUIPED
};
