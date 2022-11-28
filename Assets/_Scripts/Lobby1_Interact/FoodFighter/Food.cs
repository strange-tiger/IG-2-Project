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
    [SerializeField] Collider _collider;

    private FoodInteraction _foodInteraction;
    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);


    public override void Interact()
    {
        base.Interact();

        _foodInteraction = FindObjectOfType<PlayerInteraction>().transform.root.GetComponent<FoodInteraction>();


        if(_foodInteraction.SatietyStack != 6)
        {

            OnEated.Invoke(_foodSatietyLevel);
        
            photonView.RPC("EatFoodState", RpcTarget.All);

            StartCoroutine(RegenerateFood());

        }
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
        _collider.enabled = false;
    }

    [PunRPC]
    public void RegenerateFoodState()
    {
        _food.SetActive(true);
        _collider.enabled = true;
    }


    [PunRPC]
    public void CheckFoodState()
    {
        if (photonView.IsMine)
        {
            if (photonView.isActiveAndEnabled)
            {
                _food.SetActive(true);
                _collider.enabled = true;
            }
            else
            {
                _food.SetActive(false);
                _collider.enabled = false;
            }
        }
    }

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
}
