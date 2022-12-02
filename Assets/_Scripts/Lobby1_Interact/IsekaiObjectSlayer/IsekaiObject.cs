using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using _IRM = Defines.RPC.IsekaiRPCMethodName;

public class IsekaiObject : MonoBehaviourPun
{
    private const string WEAPON_TAG = "IsekaiWeapon";
    private const float WEAPON_VALID_VELOCITY = 1f;

    public event Action<Vector3> ObjectSlashed;

    [SerializeField] MeshRenderer _renderer;
    [SerializeField] AudioSource _audioSource;

    private static readonly WaitForSeconds FLICK_TIME = new WaitForSeconds(0.05f);
    private const float FLOAT_POINT = 1.2f;

    private bool _hitAllowed = true;

    public void ReturnHitAllowed() => _hitAllowed = true;
    private void OnEnable()
    {
        _hitAllowed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(WEAPON_TAG)
            && other.GetComponent<Rigidbody>().velocity.magnitude >= WEAPON_VALID_VELOCITY
            && _hitAllowed)
        {
            Vector3 position = other.GetComponent<SyncOVRDistanceGrabbable>().grabbedBy.transform.position;

            StartCoroutine(Vibration());

            PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, _IRM.FlickHelper);
            photonView.RPC(_IRM.FlickHelper, RpcTarget.AllBuffered);

            Debug.Log("[Isekai] " + position);

            ObjectSlashed.Invoke(position);
        }
    }

    [PunRPC]
    private void FlickHelper() => StartCoroutine(Flick());

    private IEnumerator Flick()
    {
        _audioSource.PlayOneShot(_audioSource.clip);

        _hitAllowed = false;

        int count = 3;

        while (count > 0)
        {
            _renderer.enabled = false;

            yield return FLICK_TIME;

            _renderer.enabled = true;

            yield return FLICK_TIME;

            --count;
        }

        transform.localPosition = Vector3.zero;

        gameObject.SetActive(false);
    }

    private IEnumerator Vibration()
    {
        OVRInput.SetControllerVibration(1f, 1f);

        yield return FLICK_TIME;

        OVRInput.SetControllerVibration(0f, 0f);
    }
}
