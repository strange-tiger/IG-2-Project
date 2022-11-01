//#define _Photon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Wood : FocusableObjects
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.activeSelf);
        }
        else if (stream.IsReading)
        {
            gameObject.SetActive((bool)stream.ReceiveNext());
        }
    }

    private const float COUNT_DOWN_TIME = 3f;
    private const int CAMPFIRE_LAYER = 11;

    private Coroutine _countDown;
    private OVRGrabbable _grabbable;

    private void OnEnable()
    {
        _grabbable = GetComponent<OVRGrabbable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == CAMPFIRE_LAYER)
            if (_countDown != null) StopCoroutine(_countDown);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == CAMPFIRE_LAYER)
            _countDown = StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        float countDown = 0;

        while (countDown <= COUNT_DOWN_TIME)
        {
            yield return null;
            
            if (!_grabbable.isGrabbed)
            {
                countDown += Time.deltaTime;
            }
        }

#if _Photon
        PhotonNetwork.Destroy(gameObject);
#else
        Destroy(gameObject);
#endif
    }
}
