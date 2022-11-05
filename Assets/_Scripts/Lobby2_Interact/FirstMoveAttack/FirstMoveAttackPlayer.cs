using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    [PunRPC]
    public void OnDamage()
    {
        Debug.Log("OnDamage / �����ڿ��� ȣ�� ��");
        if(PlayerControlManager.Instance.IsInvincible == true)
        {
            return;
        }
        //OVRScreenFade.instance.FadeOut();
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        StartCoroutine(Invincible(20f));
        Invoke("Revive", 1f);
    }

    public void Revive()
    {
        Debug.Log("Revive");
        //OVRScreenFade.instance.FadeIn();
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
    }

    IEnumerator Invincible(float coolTime)
    {
        float elapsedTime = 0;

        while(true)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > coolTime)
            {
                PlayerControlManager.Instance.IsInvincible = false;
                Debug.Log("���� ���� ����");
                break;
            }
            else
            {
                PlayerControlManager.Instance.IsInvincible = true;
                Debug.Log("���� ����");
            }
            yield return null;
        }
    }
}
