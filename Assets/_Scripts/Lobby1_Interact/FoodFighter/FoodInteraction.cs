using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

/*
 * 플레이어가 음식과 상호작용할 때, 플레이어의 상태를 변화시킴.
 */
public class FoodInteraction : MonoBehaviourPun, IPunObservable
{
    // 포만감 스택을 알려주는 UI에 전달할 이벤트.
    public UnityEvent OnActivateSatietyUI = new UnityEvent();
    public UnityEvent OnChangeSatietyUI = new UnityEvent();
    public UnityEvent OnDeactivateSatietyUI = new UnityEvent();

    // 플레이어의 포만감 스택.
    public int SatietyStack { get; private set; }

    private PlayerControllerMove _playerContollerMove;

    // 음식과 관련된 사운드.
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _eatingSound;

    // 100걸음을 걸으면 포만감 스택이 줄기 때문에 이를 연산하기 위한 변수.
    private Vector3 _initPosition;
    private Vector3 _nullPosition = new Vector3(0, 0, 0);

    // 포만감 스택에 따른 속도 변화와 플레이어 스케일 변화를 위한 변수.
    private float _speedSlower = 0.0001f;
    private float _fatterCharacter = 0.1f;

    // 플레이어의 걸음수.
    private float _walkCount;
    private int _dietWalkCount = 100;

    // 최대 포만감 스택.
    private int _maxSatietyStack = 6;

    /// <summary>
    /// 포만감 스택에 따른 플레이어의 스케일을 동기화하기 위해 정보를 직렬화하여 송수신.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.GetChild(2).localScale);
        }
        else if (stream.IsReading)
        {
            transform.GetChild(2).localScale = (Vector3)stream.ReceiveNext();
        }
    }

    private void OnEnable()
    {
        Food.OnEated.RemoveListener(EatFood);
        Food.OnEated.AddListener(EatFood);

        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        _playerContollerMove = GetComponent<PlayerControllerMove>();
    }

    /// <summary>
    /// 포만감 스택이 존재하면 포만감 스택을 줄여주는 메서드를 호출.
    /// </summary>
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (SatietyStack != 0)
            {
                Diet();
            }
        }
    }

    /// <summary>
    ///  걸음 수를 계산하여 포만감 스택을 줄여주는 메서드
    /// </summary>
    private void Diet()
    {
        // 메서드가 호출됐을 때의 위치를 저장.
        if (_initPosition == _nullPosition)
        {
            _initPosition = transform.position;
        }

        // 저장한 위치와 이동한 위치의 차이가 1이상이면 걸음 수를 증가시키고, 다시 위치를 저장함.
        if (Vector3.Distance(_initPosition, transform.position) >= 1)
        {
            _walkCount++;
            _initPosition = transform.position;
        }
        
        // 걸음 수가 100걸음이 되면 
        if (_walkCount == _dietWalkCount)
        {
            // 포만감 스택을 줄이고
            SatietyStack--;

            // 걸음 수를 초기화.
            _walkCount = 0;

            // 플레이어의 속도와 스케일을 다시 줄여줌.
            _playerContollerMove.MoveScale += _speedSlower;

            photonView.RPC("CharacterScaleDecrease", RpcTarget.All);

            // 스택이 0이 되었다면 UI를 비활성화하고 저장된 위치를 Null로 만들어줌.
            if (SatietyStack == 0)
            {
                OnDeactivateSatietyUI.Invoke();
                _initPosition = _nullPosition;
            }
            else
            {
                // 0이 아니라면, UI의 이미지만 바꿔줌.
                OnChangeSatietyUI.Invoke();
            }
        }
    }

    /// <summary>
    ///  음식을 먹었다는 이벤트를 받아 호출됨.
    /// </summary>
    /// <param name="foodSatietyLevel">음식의 포만감 레벨</param>
    public void EatFood(EFoodSatietyLevel foodSatietyLevel)
    {
        if (photonView.IsMine)
        {
            // 포만감 스택이 최대 스택이 아니라면
            if (SatietyStack < _maxSatietyStack)
            {
                // 정해진만큼 속도와 스케일을 조절함
                _playerContollerMove.MoveScale -= _speedSlower * (int)foodSatietyLevel;

                photonView.RPC("CharacterScaleIncrease", RpcTarget.All, (int)foodSatietyLevel);

                // 이전 스택이 0 이었다면 포만감 스택을 활성화.
                if (SatietyStack == 0)
                {
                    OnActivateSatietyUI.Invoke();
                }

                // 스택을 늘려주고
                SatietyStack += (int)foodSatietyLevel;

                // 그 이상의 스택이라면 최대 스택에 맞춰줌
                if (SatietyStack > _maxSatietyStack)
                {
                    SatietyStack = _maxSatietyStack;
                }

                // 스택에 따라 이미지를 변경시킴
                OnChangeSatietyUI.Invoke();
            }
        }
    }

    /// <summary>
    /// 포만감 스택만큼 캐릭터의 스케일을 조정함
    /// </summary>
    /// <param name="foodSatietyLevel">음식의 포만감 레벨</param>
    [PunRPC]
    public void CharacterScaleIncrease(EFoodSatietyLevel foodSatietyLevel)
    {
        // 음식을 먹었을 때, 사운드를 출력함.
        _audioSource.PlayOneShot(_eatingSound);

        transform.GetChild(2).localScale = new Vector3(transform.GetChild(2).localScale.x, transform.GetChild(2).localScale.y, transform.GetChild(2).localScale.z + (_fatterCharacter * (int)foodSatietyLevel));
    }

    /// <summary>
    /// 포만감 스택이 줄면, 스케일도 줄임.
    /// </summary>
    [PunRPC]
    public void CharacterScaleDecrease()
    {
        transform.GetChild(2).localScale = new Vector3(transform.GetChild(2).localScale.x, transform.GetChild(2).localScale.y, transform.GetChild(2).localScale.z - _fatterCharacter);
    }

    private void OnDisable()
    {
        Food.OnEated.RemoveListener(EatFood);
    }

}
