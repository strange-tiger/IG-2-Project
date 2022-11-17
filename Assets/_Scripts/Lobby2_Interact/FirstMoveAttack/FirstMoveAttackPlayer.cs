using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    public UnityEvent OnFaint = new UnityEvent();

    [PunRPC]
    public void OnDamageByBottle()
    {
        if (PlayerControlManager.Instance.IsInvincible == true)
        {
            return;
        }

        Debug.Log("OnDamageByBottle");
        GetComponentInChildren<OVRScreenFade>()?.FadeOut(0.0f);
        StartCoroutine(Invincible(20f));
        StartCoroutine(ReviveCooldown());
        OnFaint.Invoke();
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
