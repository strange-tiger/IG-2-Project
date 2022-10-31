using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public enum EFoodSatietyLevel
{
    None,
    Small,
    Big
};



public class Food : InteracterableObject, IPunObservable
{
    public static UnityEvent<EFoodSatietyLevel> OnEated = new UnityEvent<EFoodSatietyLevel>();

    [SerializeField] EFoodSatietyLevel _foodSatietyLevel;
    [SerializeField] GameObject _food;

    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);
    private BoxCollider _foodCollider;

    public void Start()
    {
        _foodCollider = GetComponentInParent<BoxCollider>();
    }


    public override void Interact()
    {
        base.Interact();

        OnEated.Invoke(_foodSatietyLevel);

        photonView.RPC("EatFoodState", RpcTarget.All);

        StartCoroutine(RegenerateFood());
    }


    public IEnumerator RegenerateFood()
    {
        yield return _waitSecondRegenerate;

        photonView.RPC("RegenerateFoodState", RpcTarget.All);

        yield return null;
    }

    [PunRPC]
    public void EatFoodState()
    {
        _food.SetActive(false);
        _foodCollider.gameObject.SetActive(false);
    }

    [PunRPC]
    public void RegenerateFoodState()
    {
        _food.SetActive(true);
        _foodCollider.gameObject.SetActive(true);
    }


    [PunRPC]
    public void CheckFoodState()
    {
        if (photonView.IsMine)
        {
            if (photonView.isActiveAndEnabled)
            {
                _food.SetActive(true);
            }
            else
            {
                _food.SetActive(false);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_food.activeSelf);
            stream.SendNext(_foodCollider.gameObject.activeSelf);
        }
        else if (stream.IsReading)
        {
            _food.SetActive((bool)stream.ReceiveNext());
            _foodCollider.gameObject.SetActive((bool)stream.ReceiveNext());
        }
    }
}
