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

    [SerializeField] int[] petEXP;
    public int[] PetEXP { get { return petEXP; } }

    [SerializeField] Vector3[] petSize;
    public Vector3[] PetSize { get { return petSize; } }

    [SerializeField] int[] petAsset;
    public int[] PetAsset { get { return petAsset; } }
}

public enum EPetStatus
{
    NONE,
    HAVE,
    EQUIPED
};
