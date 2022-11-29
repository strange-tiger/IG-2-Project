using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable/AvatarMaterialData", fileName = "AvatarMaterialData")]
public class AvatarMaterialData : ScriptableObject
{
    [SerializeField] Material[] avatarMaterial;
    public Material[] AvatarMaterial { get { return avatarMaterial; } }

    [SerializeField] Sprite[] avatarImage;
    public Sprite[] AvatarImage { get { return avatarImage; } }
}


