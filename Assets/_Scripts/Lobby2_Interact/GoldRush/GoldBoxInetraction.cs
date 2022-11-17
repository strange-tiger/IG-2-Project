using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CoinGrade = Defines.ECoinGrade;

public class GoldBoxInetraction : MonoBehaviour
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
    private GoldBoxSpawner _spawner;
    private Rigidbody _rigidbody;
    private GoldBoxSencer _sencer;
    [SerializeField] private GoldBoxEffect _effect;

    private FirstMoveAttackPlayer _playerFaintScript;
    private PlayerGoldRushInteraction _playerInteractionScript;

    private float _elapsedTime = 0f;

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

        this.enabled = false;
    }

    private void OnEnable()
    {
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        gameObject.transform.localScale = 
            new Vector3(_changedScale, _changedScale, _changedScale);

        _playerInteractionScript = transform.root.GetComponentInChildren<PlayerGoldRushInteraction>();

        _playerFaintScript = transform.root.GetComponent<FirstMoveAttackPlayer>();
        _playerFaintScript.OnFaint.RemoveListener(DropBox);
        _playerFaintScript.OnFaint.AddListener(DropBox);

        PlayerControlManager.Instance.IsInvincible = false;
    }

    private void DropBox()
    {
        gameObject.transform.localScale = _originalScale;
        transform.parent.parent = _spawner.transform;
        
        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.None;

        _sencer.enabled = true;
        this.enabled = false;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _giveGoldGrabTime)
        {
            _elapsedTime = 0f;
            _playerInteractionScript.GetGold(GiveRandomGold());
        }
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
        gameObject.transform.localScale = _originalScale;

        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        _effect.gameObject.SetActive(true);
        _effect.SetEffect(_goldCoinGiveCount[grade], grade, _spawner);

        _elapsedTime = 0f;
        this.enabled = false;
        gameObject.SetActive(false);

        return _goldCoinGiveCount[grade];
    }

    private void OnDisable()
    {
        _playerFaintScript.OnFaint.RemoveListener(DropBox);
    }
}
