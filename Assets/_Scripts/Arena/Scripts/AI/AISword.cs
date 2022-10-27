using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISword : MonoBehaviour
{
    [SerializeField]
    private Collider _swordHitBox;

    public void OnHitBox()
    {
        _swordHitBox.enabled = true;
    }

    public void OffHitBox()
    {
        _swordHitBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AI")
        {

        }
    }
}
