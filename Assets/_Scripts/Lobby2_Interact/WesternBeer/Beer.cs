using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class Beer : InteracterableObject, IPunObservable
{

    public static UnityEvent OnDrinkBeer = new UnityEvent();

    [SerializeField] GameObject _fullBeer;
    private PlayerFocus _leftRay;
    private PlayerFocus _rightRay;
    private OVRGrabbable _grabbable;
    private Vector3 _initBeerPosition;
    private YieldInstruction _regenerateTime = new WaitForSeconds(30f);
    private BeerInteraction _beerInteraction;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_fullBeer.activeSelf);
        }
        else if (stream.IsReading)
        {
            _fullBeer.SetActive((bool)stream.ReceiveNext());
        }
    }

    public void Start()
    {
        _initBeerPosition = transform.position;
        _beerInteraction = transform.root.GetComponent<BeerInteraction>();
        _grabbable = GetComponent<OVRGrabbable>();
        //_leftRay = GameObject.Find("LeftRay").GetComponent<PlayerFocus>();
        //_rightRay = GameObject.Find("rightRay").GetComponent<PlayerFocus>();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && !_beerInteraction.IsCoolTime)
        {
            OnDrinkBeer.Invoke();
            DrinkBeer();
        }
    }
    public override void Interact()
    {
        //if(_grabbable.isGrabbed)
        //{
        //    if(!_leftRay.FocusedObject.CompareTag("Player") && !_rightRay.FocusedObject.CompareTag("Player"))
        //    {
        //        base.Interact();
        //        OnDrinkBeer.Invoke();
        //        DrinkBeer();
        //    }
        //    else if(_leftRay.FocusedObject.CompareTag("Player"))
        //    {
        //        base.Interact();
        //        _leftRay.FocusedObject.gameObject.GetComponent<BeerInteraction>().CallDrinkBeer();
        //        DrinkBeer();
        //    }
        //    else if(_rightRay.FocusedObject.CompareTag("Player"))
        //    {
        //        base.Interact();
        //        _rightRay.FocusedObject.gameObject.GetComponent<BeerInteraction>().CallDrinkBeer();
        //        DrinkBeer();
        //    }
        //}
    }


    private void DrinkBeer()
    {
        _fullBeer.SetActive(false);

        StartCoroutine(ReGenerateBeer());
    }



    private IEnumerator ReGenerateBeer()
    {
        yield return _regenerateTime;

        transform.position = _initBeerPosition;
        _fullBeer.SetActive(true);

        yield return null;
    }







}
