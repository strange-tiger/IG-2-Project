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
    [SerializeField] BoxCollider _foodCollider;

    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);


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
        _foodCollider.enabled = false;
        _food.SetActive(false);
    }

    [PunRPC]
    public void RegenerateFoodState()
    {
        _foodCollider.enabled = true;
        _food.SetActive(true);
    }


    [PunRPC]
    public void CheckFoodState()
    {
        if (photonView.IsMine)
        {
            if (photonView.isActiveAndEnabled)
            {
                _foodCollider.enabled = true;
                _food.SetActive(true);

            }
            else
            {
                _foodCollider.enabled = false;
                _food.SetActive(false);

            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_food.activeSelf);
            stream.SendNext(_foodCollider.enabled);
        }
        else if (stream.IsReading)
        {
            _food.SetActive((bool)stream.ReceiveNext());
            _foodCollider.enabled = (bool)stream.ReceiveNext();
        }
    }
}
