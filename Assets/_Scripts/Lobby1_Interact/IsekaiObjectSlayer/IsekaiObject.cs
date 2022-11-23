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

    private bool _isNotFlick = true;

    public void ReturnIsNotFlick() => _isNotFlick = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(WEAPON_TAG)
            && other.GetComponent<Rigidbody>().velocity.magnitude >= WEAPON_VALID_VELOCITY
            && _isNotFlick)
        {
            Vector3 position = transform.localPosition;

            StartCoroutine(Vibration());

            photonView.RPC(_IRM.FlickHelper, RpcTarget.All);

            ObjectSlashed.Invoke(position);
        }
    }

    [PunRPC]
    private void FlickHelper() => StartCoroutine(Flick());

    private IEnumerator Flick()
    {
        _audioSource.PlayOneShot(_audioSource.clip);

        _isNotFlick = false;

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

        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, _IRM.ObjectDisabled);
        photonView.RPC(_IRM.ObjectDisabled, RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void ObjectDisabled() => gameObject.SetActive(false);

    private IEnumerator Vibration()
    {
        OVRInput.SetControllerVibration(0.7f, 0.7f);

        yield return FLICK_TIME;

        OVRInput.SetControllerVibration(0f, 0f);
    }
}
