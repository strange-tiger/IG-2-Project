using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/UserCustomizeData", fileName = "UserCustomizeData")]
public class UserCustomizeData : ScriptableObject
{
    [SerializeField] private Mesh[] avatarMesh;
    public Mesh[] AvatarMesh { get { return avatarMesh; } }

    [SerializeField] private EAvatarState[] avatarState;
    public EAvatarState[] AvatarState { get { return avatarState; } }

    [SerializeField] private int[] avatarValue;
    public int[] AvatarValue { get { return avatarValue; } }

    [SerializeField] private string[] avatarName;
    public string[] AvatarName { get { return avatarName; } }

    [SerializeField] private string[] avatarNickname;
    public string[] AvatarNickname { get { return avatarNickname; } }

    [SerializeField] private string[] avatarInfo;
    public string[] AvatarInfo { get { return avatarInfo; } }

    [SerializeField] private AvatarMaterialData[] avatarMaterial;
    public AvatarMaterialData[] AvatarMaterial { get { return avatarMaterial; } }
}

public enum EAvatarState
{
    NONE,
    HAVE,
    EQUIPED
};
