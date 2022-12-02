using UnityEngine;

// 아바타 커스터마이징에 관련된 정보를 담는 스크립터블 오브젝트
[CreateAssetMenu(menuName = "Scriptable/UserCustomizeData", fileName = "UserCustomizeData")]
public class UserCustomizeData : ScriptableObject
{
    // 각 아바타의 메쉬
    [SerializeField] private Mesh[] avatarMesh;
    public Mesh[] AvatarMesh { get { return avatarMesh; } }

    // 각 아바타의 상태 => 가지고 있지 않음, 가지고 있음, 착용중
    [SerializeField] private EAvatarState[] avatarState;
    public EAvatarState[] AvatarState { get { return avatarState; } }

    // 각 아바타의 가격
    [SerializeField] private int[] avatarValue;
    public int[] AvatarValue { get { return avatarValue; } }

    // 각 아바타의 종류
    [SerializeField] private string[] avatarName;
    public string[] AvatarName { get { return avatarName; } }

    // 각 아바타의 이름
    [SerializeField] private string[] avatarNickname;
    public string[] AvatarNickname { get { return avatarNickname; } }

    // 각 아바타의 정보
    [SerializeField] private string[] avatarInfo;
    public string[] AvatarInfo { get { return avatarInfo; } }

    // 각 아바타의 메테리얼 정보와 이미지를 가지고 있는 AvatarMaterialData 스크립터블 오브젝트
    [SerializeField] private AvatarMaterialData[] avatarMaterial;
    public AvatarMaterialData[] AvatarMaterial { get { return avatarMaterial; } }
}

public enum EAvatarState
{
    NONE,
    HAVE,
    EQUIPED
};
