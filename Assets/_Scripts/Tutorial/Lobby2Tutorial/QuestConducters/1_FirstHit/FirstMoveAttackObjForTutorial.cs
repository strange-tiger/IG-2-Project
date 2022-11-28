using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMoveAttackObjForTutorial : MonoBehaviour
{
    private Vector3 _objSpawnPos;
    private AudioSource _audioSource;

    private bool _isGrabbed = false;

    [SerializeField]
    private BoxCollider _objCollider;
    [SerializeField]
    private MeshRenderer _objMeshRenderer;

    private SyncOVRGrabber _grabber = null;
    private SyncOVRGrabbable _syncGrabbable;

    private void Awake()
    {
        _objSpawnPos = transform.position;

        _audioSource = GetComponent<AudioSource>();
        _syncGrabbable = GetComponent<SyncOVRGrabbable>();

        _syncGrabbable.CallbackOnGrabBegin = OnGrabBegin;
        _syncGrabbable.CallbackOnGrabEnd = OnGrabEnd;
    }

    private void OnEnable()
    {
        ObjPosReset();

        _objMeshRenderer.enabled = true;
        _objCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_isGrabbed)
        {
            return;
        }

        _objCollider.isTrigger = true;

        if(!other.CompareTag("FirstHitNPC"))
        {
            return;
        }

        FirstMoveAttackNPCForTurorial npc =
            other.GetComponent<FirstMoveAttackNPCForTurorial>();
        npc.onDamageByBottle();

        Crack();
    }

    public void OnGrabBegin()
    {
        _isGrabbed = true;
    }

    public void OnGrabEnd()
    {
        _isGrabbed = false;
        _objCollider.isTrigger = false;
        _grabber = null;
        ObjPosReset();
    }

    private void ObjPosReset()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = _objSpawnPos;
    }

    public void Crack()
    {
        _audioSource.Play();

        _grabber?.GrabEnd();

        _objMeshRenderer.enabled = false;
        _objCollider.enabled = false;
    }
}
