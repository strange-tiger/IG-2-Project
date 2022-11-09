using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/PetData", fileName = "PetData")]
public class PetData : ScriptableObject
{
    [SerializeField] GameObject[] petObject;
    public GameObject[] Object { get { return petObject; } }

    [SerializeField] EPetMaxExp[] maxExp;
    public EPetMaxExp[] MaxExp { get { return maxExp; } }

    [SerializeField] EPetStatus[] status;
    public EPetStatus[] Status { get { return status; } }

    [SerializeField] int[] level;
    public int[] Level { get { return level; } }

    [SerializeField] int[] exp;
    public int[] Exp { get { return exp; } }

    [SerializeField] float[] size;
    public float[] Size { get { return size; } }

    [SerializeField] int[] childIndex;
    public int[] ChildIndex { get { return childIndex; } }
}

public enum EPetStatus
{
    NONE,
    HAVE,
    EQUIPED
};

public enum EPetMaxExp
{
    NONE,
    ONEHOUR,
    THREEHOUR,
    SECONDARYEVOL,
}