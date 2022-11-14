using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class Beer : InteracterableObject, IPunObservable
{


    [SerializeField] GameObject _fullBeer;
    [SerializeField] AudioClip _drinkSound;
    [SerializeField] BoxCollider _boxCollider;
    private BoxCollider _beerCollider;
    private Vector3 _initBeerPosition;
    private YieldInstruction _regenerateTime = new WaitForSeconds(30f);
    private AudioSource _audioSource;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_fullBeer.activeSelf);
            stream.SendNext(_beerCollider.enabled);
            stream.SendNext(_boxCollider.enabled);
        }
        else if (stream.IsReading)
        {
            _fullBeer.SetActive((bool)stream.ReceiveNext());
            _beerCollider.enabled = (bool)stream.ReceiveNext();
            _boxCollider.enabled = (bool)stream.ReceiveNext();
        }
    }



    public void Start()
    {
        _initBeerPosition = transform.localPosition;
        _boxCollider = GetComponent<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void CallDrinkBeer()
    {
          photonView.RPC("DrinkBeer", RpcTarget.All);
    }

    [PunRPC]
    public void DrinkBeer()
    {
        _audioSource.PlayOneShot(_drinkSound);
        _fullBeer.SetActive(false);
        _beerCollider.enabled = false;
        _boxCollider.enabled = false;
        StartCoroutine(ReGenerateBeer());
    }



    private IEnumerator ReGenerateBeer()
    {
        yield return _regenerateTime;

        transform.position = _initBeerPosition;

        _fullBeer.SetActive(true);

        _beerCollider.enabled = true;

        _boxCollider.enabled = true;

        yield return null;
    }





}
