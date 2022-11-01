using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;
    private float _fadeTime = 2f;
    private void Awake()
    {
        _objSpawnPos = transform.position;
    }


    private void OnTriggerEnter(Collider other) // 상대를 가격한 경우
    {
        if(other.CompareTag("Player")) 
            // 근데 이러면 그냥 내 손만 닿아도 깨질 것 같은데? 오브젝트 입장에서 나 자신을 어떻게 제외시켜야할까? 예외처리 필요함
        {
            SoundManager.Instance.PlaySFXSound("쨍그랑!"); //해당 사운드 //근데 이것도 동기화 해야 할 듯
            PhotonNetwork.Destroy(gameObject);
            // other의 화면 까맣게 하기
            Stun();
            StartCoroutine(FadeIn(0, 1));
            Invoke("Respawn", _fadeTime);
        }
    }

    public void OnTriggerExit(Collider other) // 놓쳐버린 경우
    {
        if (other.CompareTag("Player"))
        {
            PhotonNetwork.Destroy(gameObject);
            Respawn();
        }
    }

    public void Stun()
    {
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
    }

    public void Respawn()
    {
        PhotonNetwork.Instantiate("bottle", _objSpawnPos, Quaternion.identity);
    }

    IEnumerator FadeIn(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            // 상대방 화면 = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / _fadeTime);
        }

        // 상대방 화면 = Mathf.Lerp(startAlpha, endAlpha, 1);
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;

        yield return null;
    }
}
