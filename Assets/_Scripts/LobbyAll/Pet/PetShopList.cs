using UnityEngine;

using EGrade = PetUIManager.PetProfile.EGrade;

[CreateAssetMenu(menuName = "Scriptable/PetShopList", fileName = "PetShopList")]
public class PetShopList : ScriptableObject
{
    [SerializeField] string[] _name;
    public string[] Name { get => _name; }

    [SerializeField] EGrade[] _grade;
    public EGrade[] Grade { get => _grade; }

    [SerializeField] string[] _explanation;
    public string[] Explanation { get => _explanation; }

    [SerializeField] int[] _price;
    public int[] Price { get => _price; }
}
