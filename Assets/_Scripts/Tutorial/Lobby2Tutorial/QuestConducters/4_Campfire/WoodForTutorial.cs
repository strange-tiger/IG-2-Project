using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodForTutorial : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private static readonly YieldInstruction SOUND_COOLTIME = new WaitForSeconds(1f);
    private const string CAMPFIRE_TAG = "Campfire";

    private SyncOVRGrabbable _grabbable;
    private bool _notOnCooltime = true;

    private CampfireQuest _questConductor;

    private void Awake()
    {
        _grabbable = GetComponent<SyncOVRGrabbable>();
        _questConductor = GetComponentInParent<CampfireQuest>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_grabbable.isGrabbed && _notOnCooltime)
        {
            _audioSource.PlayOneShot(_audioClip);
            StartCoroutine(CoAudioCoolTime());
        }
    }

    private IEnumerator CoAudioCoolTime()
    {
        _notOnCooltime = false;

        yield return SOUND_COOLTIME;

        _notOnCooltime = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
        {
            _questConductor.StackWood();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(CAMPFIRE_TAG))
        {
            _questConductor.WoodOut();
        }
    }

    private void OnDisable()
    {
        _questConductor.WoodOut();
    }
}