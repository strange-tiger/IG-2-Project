using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/PetTransformList", fileName = "PetTransformList")]
public class PetTransformList : ScriptableObject
{
    [SerializeField] Sprite[] _image;
    public Sprite[] Image { get { return _image; } }

    [SerializeField] string[] _name;
    public string[] Name { get { return _name; } }

    [SerializeField] int[] _level;
    public int[] Level { get { return _level; } }
}
