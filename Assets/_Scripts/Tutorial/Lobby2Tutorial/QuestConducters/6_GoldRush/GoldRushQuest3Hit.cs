using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRushQuest3Hit : QuestConducter
{
    [Header("GoldBox")]
    [SerializeField] private GameObject _goldBox;

    [SerializeField] private Vector3 _droppedBoxScale = new Vector3(1f, 1f, 1f);
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    
    [SerializeField] private float _dropForce = 2;
    private Rigidbody _boxRigidbody;

    private FocusableObjects _goldBoxFocusable;
    private FocusableObjectsSensor _goldBoxFocusableSencer;

    [Header("NCP")]
    [SerializeField] private GameObject _mapNPC;
    [SerializeField] private GameObject _props;
    private FirstMoveAttackNPCForTurorial _npc;

    [Header("QuestEnd")]
    [SerializeField] private float _questEndTime = 2f;
    private WaitForSeconds _waitForQuestEnd;
    


    private void Awake()
    {
        _waitForQuestEnd = new WaitForSeconds(_questEndTime);

        _boxRigidbody = _goldBox.GetComponent<Rigidbody>();
        _originalPosition = _goldBox.transform.position;
        _originalRotation = _goldBox.transform.rotation;

        _npc = _props. GetComponentInChildren<FirstMoveAttackNPCForTurorial>();
        _npc.OnNPCHit -= DropBox;
        _npc.OnNPCHit += DropBox;
    }

    public override void StartQuest()
    {
        _goldBox.SetActive(true);
        _props.SetActive(true);
        _mapNPC.SetActive(false);

        base.StartQuest();
    }

    private void DropBox()
    {
        _goldBox.transform.localScale = _droppedBoxScale;
        
        _boxRigidbody.useGravity = true;
        _boxRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _boxRigidbody.AddForce(_goldBox.transform.forward * _dropForce, ForceMode.Impulse);

        StartCoroutine(CoQuestEnd());
    }

    private IEnumerator CoQuestEnd()
    {
        yield return _waitForQuestEnd;
        QuestEnded();
    }

    private void OnDisable()
    {
        _boxRigidbody.useGravity = false;
        _boxRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        _goldBox.transform.position = _originalPosition;
        _goldBox.transform.rotation = _originalRotation;

        _goldBox.SetActive(false);
        _props.SetActive(false);
        _mapNPC.SetActive(true);
    }
}
