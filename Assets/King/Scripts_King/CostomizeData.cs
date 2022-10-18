using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/CostomizeData", fileName = "CostomizeData")]
public class CostomizeData : ScriptableObject
{

    [SerializeField] Mesh[] avatarGameObject;
    public Mesh[] AvatarGameObject { get { return avatarGameObject; } }

    [SerializeField] Material[] avatarMaterial;
    public Material[] AvatarMaterial { get { return avatarMaterial; } }


    

}

