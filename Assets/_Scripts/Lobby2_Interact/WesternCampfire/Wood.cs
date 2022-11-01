//#define _Photon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Wood : FocusableObjects
{
    private static readonly YieldInstruction COUNT_DOWN = new WaitForSeconds(3f);
    private const int CAMPFIRE_LAYER = 11;

    private Coroutine _countDown;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == CAMPFIRE_LAYER)
            StopCoroutine(_countDown);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == CAMPFIRE_LAYER)
            _countDown = StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        yield return COUNT_DOWN;

#if _Photon
        PhotonNetwork.Destroy(gameObject);
#else
        Destroy(gameObject);
#endif
    }
}
