//#define debug
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using _IRM = Defines.RPC.IsekaiRPCMethodName;

public class IsekaiObject : MonoBehaviourPun
{
    private const string WEAPON_TAG = "IsekaiWeapon";

    public event Action<Vector3> ObjectSlashed;

    [SerializeField] MeshRenderer _renderer;
    [SerializeField] AudioSource _audioSource;

    private static readonly WaitForSeconds FLICK_TIME = new WaitForSeconds(0.05f);
    private const float FLOAT_POINT = 1.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!transform.position.y.Equals(FLOAT_POINT))
        {
            return;
        }

        if (other.CompareTag(WEAPON_TAG))
        {
            Vector3 position = new Vector3(other.transform.position.x, 2f, other.transform.position.z);

            StartCoroutine(Vibration());

            photonView.RPC(_IRM.FlickHelper, RpcTarget.All, position);
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
        _audioSource.PlayOneShot(_audioSource.clip);
        
        int count = 3;

        while (count > 0)
        {
            _renderer.enabled = false;

            yield return FLICK_TIME;

            _renderer.enabled = true;

            yield return FLICK_TIME;

            --count;
        }

        if (photonView.IsMine)
        {
            ObjectSlashed.Invoke(playerPos);
        }

        transform.localPosition = Vector3.zero;

        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, _IRM.ObjectDisabled);
        photonView.RPC(_IRM.ObjectDisabled, RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void ObjectDisabled(Vector3 playerPos) => gameObject.SetActive(false);

    private IEnumerator Vibration()
    {
        OVRInput.SetControllerVibration(0.3f, 0.3f);

        yield return FLICK_TIME;

        OVRInput.SetControllerVibration(0f, 0f);
    }
}
