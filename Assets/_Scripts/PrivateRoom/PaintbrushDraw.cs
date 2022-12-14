using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PaintbrushDraw : MonoBehaviourPun, IPunObservable
{
    [SerializeField] PaintbrushReset _pad;
    [SerializeField] LayerMask _padMask;

    private const float RAY_LENGTH = 0.2f;
    private const float LINE_SCALE = 0.5f;
    private const float POINTS_DISTANCE = 0.01f;
    private static readonly Vector3 RAY_ORIGIN = new Vector3(0f, 0f, 0.1f);

    private LineRenderer _currentLineRenderer;

    private Vector3 _currentPoint = Vector3.zero;
    private Vector3 _prevPoint = Vector3.zero;

    private bool _isDraw = false;
    private int _positionCount = 2;

    private void OnEnable()
    {
        _pad.OnReset -= StopDraw;
        _pad.OnReset += StopDraw;
    }

    private void OnDisable()
    {
        _pad.OnReset -= StopDraw;
    }

    private void Update()
    {
        RaycastOnClients();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.activeSelf);
        }
        else if (stream.IsReading)
        {
            gameObject.SetActive((bool)stream.ReceiveNext());
        }
    }

    private const string PAINT_PAD_TAG = "PaintPad";
    /// <summary>
    /// 매 프레임 호출된다.
    /// 그리는 오브젝트(이하 연필)에서 나오는 레이가 그려지는 오브젝트(이하 패드)의 콜라이더와 충돌할 때, 
    /// 충돌 지점을 _currentPoint에 저장하고 DrawLine을 호출한다.
    /// 충돌하지 않으면 현재 그리고 있는 중임을 알리는 변수 _isDraw에 false를 할당한다.
    /// </summary>
    private void RaycastOnClients()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + RAY_ORIGIN, transform.forward);

        if (Physics.Raycast(ray, out hit, RAY_LENGTH, _padMask.value))
        {
            if (hit.collider.CompareTag(PAINT_PAD_TAG))
            {
                _currentPoint = hit.point;
                DrawLine();
            }
        }
        else
        {
            _isDraw = false;
        }
    }

    /// <summary>
    /// _isDraw가 false이면 true를 할당하고 CreateLine을 RPC로 호출한다.
    /// </summary>
    private void DrawLine()
    {
        if (!_isDraw)
        {
            _isDraw = true;
            
            photonView.RPC(nameof(CreateLine), RpcTarget.AllBuffered, _currentPoint);
        }
    }

    /// <summary>
    /// _positionCount에 기본 값 2를 할당하고 GenerateLineRenderer를 호출한다.
    /// _currentLineRenderer에 GenerateLineRenderer에서 반환하는 LineRenderer를 할당한다.
    /// ConnectLine을 실행한다.
    /// </summary>
    /// <param name="startPos"></param>
    [PunRPC]
    private void CreateLine(Vector3 startPos)
    {
        _positionCount = 2;

        _currentLineRenderer = GenerateLineRenderer(startPos);


        StartCoroutine(ConnectLine());
    }

    /// <summary>
    /// 프리팹 line을 생성한다.
    /// line의 라인렌더러 및 위치 정보를 할당한다.
    /// line의 라인렌더러 lineRenderer를 반환한다.
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns>LineRenderer</returns>
    private LineRenderer GenerateLineRenderer(Vector3 startPos)
    {
        GameObject line = PhotonNetwork.Instantiate("PrivateRoom\\Line", Vector3.zero, Quaternion.identity);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

        line.transform.parent = _pad.transform;
        line.transform.position = Vector3.zero;
        line.transform.localScale = LINE_SCALE * Vector3.one;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos);

        return lineRenderer;
    }

    /// <summary>
    /// _isDraw = true인 동안 ConnectLineHelper를 RPC로 호출한다.
    /// _prevPoint에 _currentPoint을 할당한다.
    /// </summary>
    /// <returns></returns>
    [PunRPC]
    private IEnumerator ConnectLine()
    {
        while (_isDraw)
        {
            photonView.RPC(nameof(ConnectLineHelper), RpcTarget.AllBuffered, _prevPoint, _currentPoint);

            _prevPoint = _currentPoint;
            yield return null;
        }
    }

    /// <summary>
    /// prevPoint가 null이 아니고 prevPoint와 currentPoint 사이 거리가 POINTS_DISTANCE 이상이면 
    /// _currentLineRenderer에 다음 점을 추가한다.
    /// 다음 점의 위치는 currentPoint가 된다.
    /// </summary>
    /// <param name="prevPoint"></param>
    /// <param name="currentPoint"></param>
    [PunRPC]
    private void ConnectLineHelper(Vector3 prevPoint, Vector3 currentPoint)
    {
        if (prevPoint != null && Mathf.Abs(Vector3.Distance(prevPoint, currentPoint)) >= POINTS_DISTANCE)
        {
            _positionCount++;
            _currentLineRenderer.positionCount = _positionCount;
            _currentLineRenderer.SetPosition(_positionCount - 1, currentPoint);
        }
    }

    /// <summary>
    /// 그리기를 멈춘다.
    /// PaintbrushReset의 OnReset을 구독한다. 
    /// _isDraw를 false로 바꾸고, CreateLine과 ConnectLineHelper의 RPC 실행 캐시를 지운다.
    /// </summary>
    private void StopDraw()
    {
        _isDraw = false;

        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(CreateLine));
        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(ConnectLineHelper));
    }
}
