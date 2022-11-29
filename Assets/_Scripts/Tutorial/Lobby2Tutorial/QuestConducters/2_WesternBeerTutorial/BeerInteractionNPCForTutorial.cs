using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OnQuestEnd = QuestConducter.QuestEnd;

public class BeerInteractionNPCForTutorial : MonoBehaviour
{
    public event OnQuestEnd OnQuestEnd;

    [SerializeField] private GameObject _effect;
    [SerializeField] private float _effectLastTime = 11f;
    private WaitForSeconds _waitForEffectLast;
    
    private AudioSource _audioSource;
    private Collider _collider;

    private int _drinkStack = 0;

    private void Awake()
    {
        _waitForEffectLast = new WaitForSeconds(_effectLastTime);
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _effect.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BeerForTutorial"))
        {
            DrinkBeer();
            other.GetComponentInParent<BeerForTutorial>().DrinkBeer();
        }
    }

    private void DrinkBeer()
    {
        ++_drinkStack;
        if(_drinkStack >= 6)
        {
            _collider.enabled = false;
            _audioSource.Play();
            _effect.SetActive(true);
            OnQuestEnd.Invoke();
        }
    }

    private IEnumerator CoEffectReset()
    {
        yield return _waitForEffectLast;
        _effect.SetActive(false);
    }

    private void OnDisable()
    {
        _drinkStack = 0;
        _collider.enabled = true;
        _effect.SetActive(false);
        _audioSource.Pause();
    }
}
