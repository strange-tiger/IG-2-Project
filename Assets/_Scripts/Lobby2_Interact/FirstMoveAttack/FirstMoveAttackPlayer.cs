using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    private bool _isGrab = false;
    private GameObject _bottle;
    private void Update()
    {
        if (false == photonView.IsMine)
        {
            return;
        }
        if (_isGrab)
        {
            Attack();
        }
    }

    // head ��ó���� �浹�� �� ������
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FirstMoveAttackObj>() == null)
        {
            return;
        }
        else
        {
            _bottle = other.gameObject;
            _isGrab = true;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<FirstMoveAttackObj>() == null)
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        _isGrab = false;
    //        _firstMoveAttackObj.photonView.RPC("Crack", RpcTarget.All, 0f);
    //    }
    //}

    private void Attack()
    {
        Debug.Log("Attack ���� ����");
        Collider[] colliders = Physics.OverlapSphere(_bottle.transform.position, 1f);

        foreach (Collider collider in colliders)
        {
            FirstMoveAttackPlayer enemy = collider.GetComponent<FirstMoveAttackPlayer>();
            if (enemy == null || enemy == this)
            {
                continue;
            }
            Debug.Log("��!!");
            enemy.photonView.RPC("OnDamage", RpcTarget.All);

            FirstMoveAttackObj firstMoveAttackObj = _bottle.GetComponent<FirstMoveAttackObj>();            
            firstMoveAttackObj.photonView.RPC("Crack", RpcTarget.All, OVRScreenFade.instance.fadeTime);
        }
    }

    [PunRPC]
    public void OnDamage()
    {
        Debug.Log("OnDamage / �����ڿ��� ȣ�� ��");
        if(PlayerControlManager.Instance.IsInvincible == true)
        {
            return;
        }
        OVRScreenFade.instance.FadeOut();
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        StartCoroutine(Invincible(20f));
        Invoke("Revive", 1f);
    }

    public void Revive()
    {
        Debug.Log("Revive");
        OVRScreenFade.instance.FadeIn();
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
