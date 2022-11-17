#define debug
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IsekaiObject : MonoBehaviourPun
{
    public event Action ObjectSlashed;

    [SerializeField] MeshRenderer _renderer;

    private static readonly WaitForSeconds FLICK_TIME = new WaitForSeconds(0.05f);
    private const float FLOAT_POINT = 1.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!transform.position.y.Equals(FLOAT_POINT))
        {
            return;
        }

        if (other.CompareTag("IsekaiWeapon"))
        {
            photonView.RPC("FlickHelper", RpcTarget.All);
        }
    }

#if debug
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Flick());
        }
    }
#endif

    [PunRPC]
    private void FlickHelper() => StartCoroutine(Flick());

    private IEnumerator Flick()
    {
        int count = 3;

        while (count > 0)
        {
            _renderer.enabled = false;

            yield return FLICK_TIME;

            _renderer.enabled = true;

            yield return FLICK_TIME;

            --count;
        }

        ObjectSlashed.Invoke();

        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
