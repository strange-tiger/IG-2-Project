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

        if (PlayerControlManager.Instance.IsInvincible == true)
        {
            return;
        }
      
        GetComponentInChildren<OVRScreenFade>().FadeOut(0.0f);
        StartCoroutine(Invincible(20f));
        StartCoroutine(ReviveCooldown());
    }

    public void Revive()
    {
        GetComponentInChildren<OVRScreenFade>().FadeIn(2.0f);
    }

    IEnumerator Invincible(float coolTime)
    {
        float elapsedTime = 0;

        while (true)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= coolTime)
            {
                PlayerControlManager.Instance.IsInvincible = false;
                elapsedTime = 0;
                Debug.Log("무적 상태 해제");
                break;
            }
            else
            {
                PlayerControlManager.Instance.IsInvincible = true;
            }
            yield return null;
        }
    }

    YieldInstruction _reviveCooldown = new WaitForSeconds(2.0f);
    IEnumerator ReviveCooldown()
    {
        yield return _reviveCooldown;
        Revive();
    }
}
