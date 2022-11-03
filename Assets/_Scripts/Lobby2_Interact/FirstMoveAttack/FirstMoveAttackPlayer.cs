using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    private void Update()
    {
        if (false == photonView.IsMine)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.J)) // ±×·¦À» ÇßÀ» ¶§
        {
            Attack();
        }
    }

    private void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider collider in colliders)
        {
            FirstMoveAttackPlayer enemy = collider.GetComponent<FirstMoveAttackPlayer>();
            if (enemy == null || enemy == this)
            {
                continue;
            }
            enemy.photonView.RPC("OnDamage", RpcTarget.All);
        }
    }

    [PunRPC]
    public void OnDamage()
    {
        // È­¸é ¹Ù·Î ²¨Áü
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        Invoke("Revive", 1f);
    }

    public void Revive()
    {
        OVRScreenFade.instance.FadeIn();
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
    }
}
