using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/PetData", fileName = "PetData")]
public class PetData : ScriptableObject
{
    [SerializeField] GameObject[] _object;
    public GameObject[] Object { get { return _object; } }

    [SerializeField] EPetMaxExp[] _maxExp;
    public EPetMaxExp[] MaxExp { get { return _maxExp; } }

    [SerializeField] EPetStatus[] _status;
    public EPetStatus[] Status { get { return _status; } }

    [SerializeField] int[] _level;
    public int[] Level { get { return _level; } }

    [SerializeField] int[] _exp;
    public int[] Exp { get { return _exp; } }

    [SerializeField] float[] _size;
    public float[] Size { get { return _size; } }

    [SerializeField] int[] _childIndex;
    public int[] ChildIndex { get { return _childIndex; } }
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