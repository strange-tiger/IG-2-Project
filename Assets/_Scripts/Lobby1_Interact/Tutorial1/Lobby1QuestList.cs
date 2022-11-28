using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Lobby1List", fileName = "Lobby1List")]
public class Lobby1QuestList : ScriptableObject
{
    [SerializeField] private string[] _dialogue;
    public string[] Dialogue { get { return _dialogue; } }

    [SerializeField] private string[] _isQuest;
    public string[] IsQuest { get { return _isQuest; } }
}
