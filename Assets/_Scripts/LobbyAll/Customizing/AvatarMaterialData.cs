using UnityEngine;
using UnityEngine.UI;

// 아바타에게 맞는 메테리얼과 상점 / 장착 UI 에서 보여줄 이미지를 저장하는 스크립터블 오브젝트
[CreateAssetMenu(menuName = "Scriptable/AvatarMaterialData", fileName = "AvatarMaterialData")]
public class AvatarMaterialData : ScriptableObject
{
    // 각 아바타에 맞는 메테리얼
    [SerializeField] private Material[] avatarMaterial;
    public Material[] AvatarMaterial { get { return avatarMaterial; } }

    // 각 아바타에 맞는 이미지 Sprite
    [SerializeField] private Sprite[] avatarImage;
    public Sprite[] AvatarImage { get { return avatarImage; } }
}


