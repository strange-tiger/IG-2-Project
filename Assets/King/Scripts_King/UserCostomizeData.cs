using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/UserCostomizeData", fileName = "UserCostomizeData")]
public class UserCostomizeData : ScriptableObject
{

    [SerializeField] EAvatarState[] avatarState;
    public EAvatarState[] AvatarState { get { return avatarState; } }
    [SerializeField] int[] userMaterial;
    public int[] UserMaterial { get { return userMaterial; } }

}

public enum EAvatarState
{
    NONE,
    HAVE,
    EQUIPED
};
