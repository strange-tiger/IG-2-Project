using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

/*
 * OVRGrabber를 동기화하기위해 만든 스크립트.
 * OVRGrabbable을 그대로 가져왔으며, Grab 상태에 따라 GrabbedObject의 Collider를 IsTrigger로 변경해주는 코드가 추가됨.
 */
public class SyncOVRGrabbable : MonoBehaviourPun
{
    [SerializeField]
    protected bool m_allowOffhandGrab = true;
    [SerializeField]
    protected bool m_snapPosition = false;
    [SerializeField]
    protected bool m_snapOrientation = false;
    [SerializeField]
    protected Transform m_snapOffset;
    [SerializeField]
    protected Collider[] m_grabPoints = null;

    protected bool m_grabbedKinematic = false;
    protected Collider m_grabbedCollider = null;
    protected SyncOVRGrabber m_grabbedBy = null;

    public UnityEvent CallbackOnGrabBegin { get; set; } = new UnityEvent();
    public UnityEvent<SyncOVRGrabber> CallbackOnGrabHand { get; set; } = new UnityEvent<SyncOVRGrabber>();
    public UnityEvent CallbackOnGrabEnd { get; set; } = new UnityEvent();
    public UnityEvent<PhotonView, SyncOVRGrabber> CallbackGrabberSetting { get; set; } = new UnityEvent<PhotonView, SyncOVRGrabber>();

    /// <summary>
    /// If true, the object can currently be grabbed.
    /// </summary>
    public bool allowOffhandGrab
    {
        get { return m_allowOffhandGrab; }
    }

    /// <summary>
    /// If true, the object is currently grabbed.
    /// </summary>
    public bool isGrabbed
    {
        get { return m_grabbedBy != null; }
    }

    /// <summary>
    /// If true, the object's position will snap to match snapOffset when grabbed.
    /// </summary>
    public bool snapPosition
    {
        get { return m_snapPosition; }
    }

    /// <summary>
    /// If true, the object's orientation will snap to match snapOffset when grabbed.
    /// </summary>
    public bool snapOrientation
    {
        get { return m_snapOrientation; }
    }

    /// <summary>
    /// An offset relative to the OVRGrabber where this object can snap when grabbed.
    /// </summary>
    public Transform snapOffset
    {
        get { return m_snapOffset; }
    }

    /// <summary>
    /// Returns the OVRGrabber currently grabbing this object.
    /// </summary>
    public SyncOVRGrabber grabbedBy
    {
        get { return m_grabbedBy; }
    }

    /// <summary>
    /// The transform at which this object was grabbed.
    /// </summary>
    public Transform grabbedTransform
    {
        get { return m_grabbedCollider.transform; }
    }

    /// <summary>
    /// The Rigidbody of the collider that was used to grab this object.
    /// </summary>
    public Rigidbody grabbedRigidbody
    {
        get { return m_grabbedCollider.attachedRigidbody; }
    }

    /// <summary>
    /// The contact point(s) where the object was grabbed.
    /// </summary>
    public Collider[] grabPoints
    {
        get { return m_grabPoints; }
    }

    /// <summary>
    /// Notifies the object that it has been grabbed.
    /// </summary>
    virtual public void GrabBegin(SyncOVRGrabber hand, Collider grabPoint)
    {
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;


        CallbackOnGrabBegin?.Invoke();
        CallbackOnGrabHand?.Invoke(hand);
        CallbackGrabberSetting?.Invoke(hand.transform.root.gameObject.GetPhotonView(), hand.GetComponent<SyncOVRGrabber>());

        //photonView.RPC(nameof(GrabObject), RpcTarget.AllBuffered, true);
        gameObject.GetComponentInChildren<Collider>().isTrigger = true;
        //gameObject.GetComponentInChildren<Rigidbody>().useGravity = false;
    }

    /// <summary>
    /// Notifies the object that it has been released.
    /// </summary>
    virtual public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;

        m_grabbedBy = null;
        m_grabbedCollider = null;

        CallbackOnGrabEnd?.Invoke();

        //photonView.RPC(nameof(GrabObject), RpcTarget.AllBuffered, false);
        gameObject.GetComponent<Collider>().isTrigger = false;
        //gameObject.GetComponentInChildren<Rigidbody>().useGravity = true;

    }


    void Awake()
    {
        if (m_grabPoints.Length == 0)
        {
            // Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            if (collider == null)
            {
                throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            // Create a default grab point
            m_grabPoints = new Collider[1] { collider };
        }
    }

    protected virtual void Start()
    {
        m_grabbedKinematic = GetComponent<Rigidbody>().isKinematic;
    }

    void OnDestroy()
    {
        if (m_grabbedBy != null)
        {
            // Notify the hand to release destroyed grabbables
            m_grabbedBy.ForceRelease(this);
        }
    }
}
