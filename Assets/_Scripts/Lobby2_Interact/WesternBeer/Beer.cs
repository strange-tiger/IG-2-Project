using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class Beer : InteracterableObject
{
    [Header("Beer")]
    [SerializeField] private GameObject _fullBeer;
    [SerializeField] private BoxCollider _grabCollider;

    private Vector3 _initBeerPosition;
    private YieldInstruction _regenerateTime = new WaitForSeconds(30f);



    /// <summary>
    /// 초기 맥주의 위치를 저장하여 다시 맥주가 만들어 질때의 위치를 정할 수 있도록 만듬.
    /// </summary>
    private void Start()
    {
        _initBeerPosition = transform.position;
    }

    /// <summary>
    /// 맥주와의 상호작용을 RPC 함수로 호출.
    /// </summary>
    public void CallDrinkBeer()
    {
        photonView.RPC("DrinkBeer", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Collider와 Object를 DeAcitive하고, 다시 만드는 코루틴을 시작함.
    /// </summary>
    [PunRPC]
    public void DrinkBeer()
    {
        _fullBeer.SetActive(false);
        _grabCollider.enabled = false;
        StartCoroutine(ReGenerateBeer());
    }

    
    /// <summary>
    /// 시간이 지나면 처음 저장했던 위치에서 맥주가 재생성됨.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReGenerateBeer()
    {
        yield return _regenerateTime;

        transform.position = _initBeerPosition;

        _fullBeer.SetActive(true);

        _grabCollider.enabled = true;

        yield return null;
    }
}
