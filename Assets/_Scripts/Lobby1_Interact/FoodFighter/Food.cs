using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

/*
 * Food와 플레이어가 상호작용 할 수 있게 함
 */
public class Food : InteracterableObject, IPunObservable
{
    // 플레이어에게 음식의 포만감 레벨과 먹었음을 전달하는 이벤트
    public static UnityEvent<EFoodSatietyLevel> OnEated = new UnityEvent<EFoodSatietyLevel>();

    [Header("Food Info")]
    [SerializeField] private EFoodSatietyLevel _foodSatietyLevel;
    [SerializeField] private GameObject _food;
    [SerializeField] private Collider _collider;

    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);

    private FoodInteraction _foodInteraction;

    /// <summary>
    /// 음식의 Collider와 Object의 활성화 여부를 직렬화하여 송수신
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_food.activeSelf);
            stream.SendNext(_collider.enabled);
        }
        else if (stream.IsReading)
        {
            _food.SetActive((bool)stream.ReceiveNext());
            _collider.enabled = (bool)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// 플레이어가 음식과 상호작용하면 상호작용한 플레이어의 OnEated 이벤트 호출과 음식의 Collider와 Object를 비활성화 시킴.
    /// 또한, 음식이 60초 뒤에 재생성되는 코루틴을 시작함.
    /// </summary>
    public override void Interact()
    {
        base.Interact();

        _foodInteraction = FindObjectOfType<PlayerInteraction>().transform.root.GetComponent<FoodInteraction>();

        if (_foodInteraction.SatietyStack != 6)
        {
            OnEated.Invoke(_foodSatietyLevel);

            photonView.RPC(nameof(EatFoodState), RpcTarget.All);

            StartCoroutine(CoRegenerateFood());
        }
    }

    /// <summary>
    /// 음식을 비활성화 시키는 RPC 함수.
    /// </summary>
    [PunRPC]
    public void EatFoodState()
    {
        _food.SetActive(false);
        _collider.enabled = false;
    }

    /// <summary>
    /// 상호작용된 음식이 재생성되게 하는 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoRegenerateFood()
    {
        yield return _waitSecondRegenerate;

        photonView.RPC(nameof(RegenerateFoodState), RpcTarget.All);

        yield return null;
    }

    /// <summary>
    /// 음식을 재생성시키는 RPC 함수.
    /// </summary>
    [PunRPC]
    public void RegenerateFoodState()
    {
        _food.SetActive(true);
        _collider.enabled = true;
    }
}
public enum EFoodSatietyLevel
{
    None,
    Small,
    Big
};
