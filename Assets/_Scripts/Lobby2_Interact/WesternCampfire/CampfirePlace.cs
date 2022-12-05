using UnityEngine;
using Photon.Pun;

public class CampfirePlace : MonoBehaviour
{
    private const string CAMPFIRE_TAG = "Campfire";
    private const string STOP_COUNTDOWN = "StopCountDown";
    private const string START_COUNTDOWN = "StartCountDown";

    /// <summary>
    /// 장작과 충돌하면 장작이 사라지지 않게 한다.
    /// 
    /// 태그가 "Campfire"인 콜라이더와 충돌하면 내부의 코드를 실행한다.
    /// 충돌한 콜라이더의 Wood 컴포넌트를 받아, "StopCountDown"을 호출한다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
        {
            other.GetComponent<Wood>().photonView.RPC(STOP_COUNTDOWN, RpcTarget.All);
        }
    }

    /// <summary>
    /// 장작이 벗어나면 장작이 사라질 카운트다운을 시작한다.
    /// 
    /// 태그가 "Campfire"인 콜라이더로부터 벗어나면 내부의 코드를 실행한다.
    /// 충돌한 콜라이더의 Wood 컴포넌트를 받아, "StartCountDown"을 호출한다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
        {
            other.GetComponent<Wood>().photonView.RPC(START_COUNTDOWN, RpcTarget.All);
        }
    }
}
