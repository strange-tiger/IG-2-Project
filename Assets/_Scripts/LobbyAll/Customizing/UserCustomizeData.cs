using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/UserCustomizeData", fileName = "UserCustomizeData")]
public class UserCustomizeData : ScriptableObject
{
    [SerializeField] Mesh[] avatarMesh;
    public Mesh[] AvatarMesh { get { return avatarMesh; } }

    [SerializeField] EAvatarState[] avatarState;
    public EAvatarState[] AvatarState { get { return avatarState; } }

    [SerializeField] int[] userMaterial;
    public int[] UserMaterial { get { return userMaterial; } }
    [SerializeField] int[] avatarValue;
    public int[] AvatarValue { get { return avatarValue; } }
    [SerializeField] string[] avatarName;
    public string[] AvatarName { get { return avatarName; } }
}

public enum EAvatarState
{
    NONE,
    HAVE,
    EQUIPED
};
