using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager: GlobalInstance<PlayerControlManager>
{
    public bool IsMoveable { get; set; } = true;
    public bool IsRayable { get; set; } = true;
}
