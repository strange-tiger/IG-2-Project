using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/StartRoomList", fileName ="StartRoomList")]
public class StartRoomQuestList : ScriptableObject
{
    [SerializeField] private string[] _dialogue;
    public string[] Dialogue { get { return _dialogue; } }

    [SerializeField] private int[] _isQuest;
    public int[] IsQuest { get { return _isQuest; } }
}
