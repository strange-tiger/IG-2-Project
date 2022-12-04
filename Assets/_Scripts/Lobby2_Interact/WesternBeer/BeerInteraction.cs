using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;

/*
 * 플레이어의 PlayerMouse에 부착되어 있으며, 플레이어가 맥주와 상호작용이 가능하게끔 함.
 */

public class BeerInteraction : MonoBehaviourPun
{

    [Header("Drink Sound")]
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _drinkSound;

    private YieldInstruction _coolTime = new WaitForSeconds(10f);

    private PlayerControllerMove _playerContollerMove;

    // 만취 상태의 이펙트, 화면 페이드 인, 아웃을 담당하는 컴포넌트.
    private PlayerDebuffManager _playerDebuff;

    // 초기 플레이어 화면의 메테리얼 컬러.
    private Color _initUIColor = new Color(1f, 1f, 0.28f, 0f);

    // 취기 스택은 -1 부터 시작함.
    private int _drinkStack = -1;

    // 맥주를 마셨을 때의 쿨타임과 플레이어의 속도를 조절해줌.
    private bool _isCoolTime;
    private bool _isTrembling;
    private float[] _tremblingSpeed = new float[2];

    // 특정 시간마다 취기스택을 줄이거나, 속도가 조절되는 시간.
    private float _soberUpElapsedTime;
    private float _tremblingElapsedTime;

    // 플레이어의 초기 속도.
    private float _initPlayerSpeed = 1.0f;

    private void Start()
    {
        _playerContollerMove = GetComponentInParent<PlayerControllerMove>();
        _playerDebuff = GetComponentInParent<PlayerDebuffManager>();
        _audioSource = transform.root.GetChild(1).GetComponent<AudioSource>();
    }

    /// <summary>
    /// PlayerMouse에 맥주를 충돌 시키면 DrinkBeer를 호출함.
    /// </summary>
    /// <param name="other"> PlayerMouse와 충돌한 Collider </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beer"))
        {
            if (!_isCoolTime)
            {
                photonView.RPC("CallDrinkBeer", RpcTarget.All);
                other.GetComponentInParent<Beer>().CallDrinkBeer();
            }
        }
    }

    /// <summary>
    /// 취기 스택을 줄여주는 시간 연산을 위해 Update 사용.
    /// </summary>
    private void Update()
    {
        if (_drinkStack > -1)
        {
            _soberUpElapsedTime += Time.deltaTime;

            // 맥주를 마시고 60초가 지나면 취기 스택이 감소
            if (_soberUpElapsedTime >= 60)
            {
                SoberUp();
            }

            // 취기 스택이 있으면, 5초마다 속도가 증가, 감소를 반복함
            _tremblingElapsedTime += Time.deltaTime;

            if (_tremblingElapsedTime > 5f && _drinkStack > 0)
            {
                // 취기가 있으면 5초마다 속도를 변경하여 휘청거림을 표현함
                _isTrembling = !_isTrembling;

                _tremblingElapsedTime = 0f;

                
                if (_isTrembling)
                {
                    _playerContollerMove.MoveScale = _tremblingSpeed[0];
                }
                else
                {
                    _playerContollerMove.MoveScale = _tremblingSpeed[1];
                }
            }
        }
    }


    /// <summary>
    /// 시간이 경과하면 취기가 떨어지게하는 메서드.
    /// </summary>
    private void SoberUp()
    {
        if (photonView.IsMine)
        {
            // 취기 스택을 줄이고
            _drinkStack--;

            // 술이 깨는 시간을 초기화.
            _soberUpElapsedTime = 0;

            // 화면의 색을 연하게 해주고
            _playerDebuff.FadeMaterial.color = new Color(1f, 1f, 0.28f, (0f + (0.1f * _drinkStack)));
            
            // 휘청거리는 정도도 낮춰줌.
            _tremblingSpeed[0] = _initPlayerSpeed + (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
            _tremblingSpeed[1] = _initPlayerSpeed - (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
        }
    }

    /// <summary>
    /// 맥주와의 상호작용을 알리는 RPC 함수.
    /// </summary>
    [PunRPC]
    public void CallDrinkBeer()
    {
        if (photonView.IsMine)
        {
            DrinkBeer();
        }
    }

    /// <summary>
    /// 맥주와 상호작용하는 메서드.
    /// </summary>
    private void DrinkBeer()
    {
        if (photonView.IsMine)
        {
            // 맥주를 마시면 사운드를 출력하고
            _audioSource.PlayOneShot(_drinkSound);

            // 쿨타임을 활성화하고
            _isCoolTime = true;

            // 취기 스택을 올림.
            _drinkStack++;

            // 술이 깨는 시간을 초기화.
            _soberUpElapsedTime = 0;

            // 속도를 초기화.
            _playerContollerMove.MoveScale = _initPlayerSpeed;

            // 쿨타임이 지나면 다시 쿨타임을 비활성화 시키는 코루틴 시작.
            StartCoroutine(CoCoolTime());

            // 취기 스택이 6이면 기절함.
            if (_drinkStack == 5)
            {
                DrunkenDebuff();
            }

            // 취기 스택이 2이상이면 휘청거림과 화면이 노랗게 변하고, 이는 스택에 따라 정도가 심해짐.
            else if (_drinkStack < 5 && _drinkStack > 0)
            {
                _playerDebuff.FadeMaterial.color = new Color(1f, 1f, 0.28f, (0f + (0.1f * _drinkStack)));

                _tremblingSpeed[0] = _initPlayerSpeed + (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
                _tremblingSpeed[1] = _initPlayerSpeed - (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
            }
        }
    }

    /// <summary>
    /// 쿨타임이 지나면 다시 쿨타임을 비활성화 시켜주는 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoCoolTime()
    {
        yield return _coolTime;

        _isCoolTime = false;

    }

    /// <summary>
    /// 플레이어가 만취 상태일 때 기절 시키는 메서드.
    /// </summary>
    private void DrunkenDebuff()
    {
        // PlayerDebuffManager에서 만취 디버프 메서드를 호출.
        _playerDebuff.CallDrunkenDebuff();

        // 취기 스택은 0으로 만들어줌.
        _drinkStack = -1;

        // 화면도 초기화.
        _playerDebuff.FadeMaterial.color = _initUIColor;
        
        // 플레이어의 속도 초기화.
        _tremblingSpeed[0] = _initPlayerSpeed;
        _tremblingSpeed[1] = _initPlayerSpeed;
    }
}
