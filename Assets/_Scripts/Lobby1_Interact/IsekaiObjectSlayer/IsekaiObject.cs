#define debug
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IsekaiObject : MonoBehaviourPun
{
    public event Action<Vector3> ObjectSlashed;

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
            photonView.RPC("FlickHelper", RpcTarget.All, other.transform.position);
        }
    }

#if debug
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Flick(new Vector3(1f, 2f, 0f)));
        }
    }
#endif

    [PunRPC]
    private void FlickHelper(Vector3 playerPos) => StartCoroutine(Flick(playerPos));

    private IEnumerator Flick(Vector3 playerPos)
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

        ObjectSlashed.Invoke(playerPos);

        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
