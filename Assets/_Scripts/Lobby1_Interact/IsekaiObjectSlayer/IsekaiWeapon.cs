using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsekaiWeapon : MonoBehaviour
{
    private SyncOVRGrabbable _grabbable;

    private void Awake()
    {
        _grabbable = GetComponent<SyncOVRGrabbable>();
    }

}
