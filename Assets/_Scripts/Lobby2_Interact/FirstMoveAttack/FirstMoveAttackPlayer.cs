using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    YieldInstruction _reviveCooldown = new WaitForSeconds(2.0f);

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

        Debug.Log("IsDamaged");
      
        GetComponentInChildren<OVRScreenFade>().FadeOut(0.0f);

        StartCoroutine(Invincible(20f));
        StartCoroutine(ReviveCooldown());
    }

    public void Revive()
    {
        Debug.Log("Revive");

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
                Debug.Log("公利 惑怕 秦力");
                break;
            }
            else
            {
                PlayerControlManager.Instance.IsInvincible = true;
                Debug.Log("公利 惑怕");
            }
            yield return null;
        }
    }

    IEnumerator ReviveCooldown()
    {
        yield return _reviveCooldown;
        Revive();
    }
}
