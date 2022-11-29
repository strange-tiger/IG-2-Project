using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;


public class Food : InteracterableObject, IPunObservable
{
    public static UnityEvent<EFoodSatietyLevel> OnEated = new UnityEvent<EFoodSatietyLevel>();

    [SerializeField] private EFoodSatietyLevel _foodSatietyLevel;
    [SerializeField] private GameObject _food;
    [SerializeField] private Collider _collider;

    private static readonly YieldInstruction _waitSecondRegenerate = new WaitForSeconds(60f);

    private FoodInteraction _foodInteraction;

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

    public override void Interact()
    {
        base.Interact();

        _foodInteraction = FindObjectOfType<PlayerInteraction>().transform.root.GetComponent<FoodInteraction>();

        if (_foodInteraction.SatietyStack != 6)
        {
            OnEated.Invoke(_foodSatietyLevel);

            photonView.RPC("EatFoodState", RpcTarget.All);

            StartCoroutine(CoRegenerateFood());
        }
    }

    [PunRPC]
    public void EatFoodState()
    {
        _food.SetActive(false);
        _collider.enabled = false;
    }

    public IEnumerator CoRegenerateFood()
    {
        yield return _waitSecondRegenerate;

        photonView.RPC("RegenerateFoodState", RpcTarget.All);

        yield return null;
    }

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
