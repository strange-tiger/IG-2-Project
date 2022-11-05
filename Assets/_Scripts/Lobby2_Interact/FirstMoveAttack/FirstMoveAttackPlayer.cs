using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    [PunRPC]
    public void OnDamage()
    {
        Debug.Log("OnDamage / 피해자에게 호출 됨");

        if (PlayerControlManager.Instance.IsInvincible == true)
        {
            return;
        }

        if (photonView.IsMine)
        {
            Debug.Log("IsDamaged");
            PlayerControlManager.Instance.IsMoveable = false;
            PlayerControlManager.Instance.IsRayable = false;
        }

        StartCoroutine(Invincible(20f));

        // 2초 딜레이 코루틴
        Revive();
    }

    public void Revive()
    {
        Debug.Log("Revive");    

        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
    }

    IEnumerator Invincible(float coolTime)
    {
        float elapsedTime = 0;

        while (true)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > coolTime)
            {
                PlayerControlManager.Instance.IsInvincible = false;
                Debug.Log("무적 상태 해제");
                break;
            }
            else
            {
                PlayerControlManager.Instance.IsInvincible = true;
                Debug.Log("무적 상태");
            }
            yield return null;
        }
    }
}
