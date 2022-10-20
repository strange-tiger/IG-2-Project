using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/CustomizeData", fileName = "CustomizeData")]
public class CustomizeData : ScriptableObject
{

    [SerializeField] Material[] avatarMaterial;
    public Material[] AvatarMaterial { get { return avatarMaterial; } }

}

