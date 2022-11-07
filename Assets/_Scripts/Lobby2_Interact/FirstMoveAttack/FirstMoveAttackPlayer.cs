using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    [PunRPC]
    public void OnDamageByBottle(int damagedPlayerID)
    {
        if (PhotonNetwork.GetPhotonView(damagedPlayerID).IsMine == false)
        {
            return;
        }

        Debug.Log("OnDamage / 피해자에게 호출 됨");

        if (PlayerControlManager.Instance.IsInvincible == true)
        {
            return;
        }

        Debug.Log("IsDamaged");
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        GetComponentInChildren<OVRScreenFade>().FadeOut(0.0f);

        StartCoroutine(Invincible(20f));

        StartCoroutine(Cooldown(2.0f));

    }

    public void Revive()
    {
        Debug.Log("Revive");

        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
        GetComponentInChildren<OVRScreenFade>().FadeIn(2.0f);
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

    IEnumerator Cooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
