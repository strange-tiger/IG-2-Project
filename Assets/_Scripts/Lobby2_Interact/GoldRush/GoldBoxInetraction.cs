using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using CoinGrade = Defines.ECoinGrade;

public class GoldBoxInetraction : MonoBehaviourPunCallbacks
{
    [Header("Gold")]
    [SerializeField] private int[] _goldCoinGiveCount = new int[(int)CoinGrade.Max];
    [SerializeField] private float[] _goldCoinRate = new float[(int)CoinGrade.Max];
    private float _maxGoldCoinRate;

    [Header("SetScale")]
    [SerializeField] private float _changedScale = 0.5f;

    [Header("Grab Time")]
    [SerializeField] private float _giveGoldGrabTime = 60f;

    public UnityEvent OnGiveGold = new UnityEvent();

    private Rigidbody _rigidbody;
    private GoldBoxSpawner _spawner;
    private GoldBoxSencer _sencer;
    [SerializeField] private GoldBoxEffect _effect;
    [SerializeField] private float _dropForce = 2;

    private PlayerGoldRushInteraction _playerInteractionScript;

    private float _elapsedTime = 0f;
    private float _ElapsedTime
    {
        get
        {
            return _elapsedTime;
        }

        set
        {
            photonView.RPC(nameof(SetElapsedTime), RpcTarget.AllBuffered, value);
        }
    }

    private readonly Vector3 _originalScale = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        _spawner = transform.root.GetComponent<GoldBoxSpawner>();
        _rigidbody = transform.parent.GetComponent<Rigidbody>();
        _sencer = transform.parent.GetComponent<GoldBoxSencer>();

        // È®·ü °è»êÀ» À§ÇÑ ÃÑ È®·ü ±¸ÇÏ±â
        _maxGoldCoinRate = 0f;
        foreach (float rate in _goldCoinRate)
        {
            _maxGoldCoinRate += rate;
        }
    }

    public override void OnJoinedRoom()
    {
        EnableScript(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        _sencer.photonView.RequestOwnership();

        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        //gameObject.transform.localScale = 
        //    new Vector3(_changedScale, _changedScale, _changedScale);
        photonView.RPC(nameof(SetLocalScale), RpcTarget.All, new Vector3(_changedScale, _changedScale, _changedScale));

        _playerInteractionScript = transform.root.GetComponentInChildren<PlayerGoldRushInteraction>();

        PlayerControlManager.Instance.IsInvincible = false;
    }

    [PunRPC]
    private void SetLocalScale(Vector3 newScale)
    {
        gameObject.transform.localScale = newScale;
    }

    private void DropBox()
    {
        photonView.RPC(nameof(BoxDropped), RpcTarget.AllBuffered);

        //gameObject.transform.localScale = _originalScale;
        photonView.RPC(nameof(SetLocalScale), RpcTarget.All, _originalScale);
        transform.parent.parent = _spawner.transform;

        Debug.Log("µé¾î¿È");
        //_sencer.enabled = true;
        _sencer.EnableScript(true);
        //this.enabled = false;
        EnableScript(false);
    }

    [PunRPC]
    private void BoxDropped()
    {
        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.AddForce(transform.forward * _dropForce, ForceMode.Impulse);
    }

    private void Update()
    {
        if (PlayerControlManager.Instance.IsInvincible)
        {
            DropBox();
        }
        
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _giveGoldGrabTime)
        {
            _elapsedTime = 0f;
            _playerInteractionScript.GetGold(GiveRandomGold());
        }
        Debug.Log($"[GoldRush] GrabbedTime {_elapsedTime}");
    }

    private int GiveRandomGold()
    {
        OnGiveGold.Invoke();
        float randomInt = Random.Range(0f, _maxGoldCoinRate);

        float coinRate = 0f;
        for (int i = 0; i < (int)CoinGrade.Max; ++i)
        {
            coinRate += _goldCoinRate[i];
            if (randomInt < coinRate)
            {
                return GiveCoinEffect(i);
            }
        }

        return -1;
    }

    private int GiveCoinEffect(int grade)
    {
        //gameObject.transform.localScale = _originalScale;
        photonView.RPC(nameof(SetLocalScale), RpcTarget.All, _originalScale);

        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        _effect.SetEffect(_goldCoinGiveCount[grade], grade, _spawner);
        _effect.SetActiveObject(true);
        //_effect.gameObject.SetActive(true);

        _elapsedTime = 0f;
        //this.enabled = false;
        //gameObject.SetActive(false);
        EnableScript(false);
        SetActiveObject(false);

        return _goldCoinGiveCount[grade];
    }

    public void EnableScript(bool value)
    {
        photonView.RPC(nameof(EnableScriptByRPC), RpcTarget.AllBuffered, value);
    }
    [PunRPC]
    private void EnableScriptByRPC(bool value)
    {
        Debug.Log($"[GoldRush] Interaction Script {value}");
        this.enabled = value;
    }

    public void SetActiveObject(bool value)
    {
        photonView.RPC(nameof(SetActiveObjectByRPC), RpcTarget.AllBuffered, value);
    }
    [PunRPC]
    private void SetActiveObjectByRPC(bool value)
    {
        Debug.Log($"[GoldRush] Interaction Obejct {value}");
        gameObject.SetActive(value);
    }

    [PunRPC]
    private void SetElapsedTime(int elapsedTime)
    {
        _elapsedTime = elapsedTime;
    }
}
