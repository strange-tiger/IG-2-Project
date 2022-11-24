using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Lobby1List", fileName = "Lobby1List")]
public class Lobby1QuestList : ScriptableObject
{
    [SerializeField] private string[] _dialogue;
    public string[] Dialogue { get { return _dialogue; } }

    [SerializeField] private bool[] _isQuest;
    public bool[] IsQuest { get { return _isQuest; } }
}
