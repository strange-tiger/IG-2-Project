using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/CostomizeData", fileName = "CostomizeData")]
public class CostomizeData : ScriptableObject
{

    [SerializeField]
    private Mesh[] avatarGameObject;
    public Mesh[] AvatarGameObject { get { return avatarGameObject; } }



}