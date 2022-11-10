#define _Photon
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
        Debug.DrawRay(transform.position + RAY_ORIGIN, transform.forward);

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

    private void RaycastOnClients()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + RAY_ORIGIN, transform.forward);

        if (Physics.Raycast(ray, out hit, RAY_LENGTH, _padMask.value))
        {
            _currentPoint = hit.point;
            DrawLine();
        }
        else
        {
            _isDraw = false;
        }
    }

    private void DrawLine()
    {
        if (!_isDraw)
        {
            _isDraw = true;
#if _Photon
            photonView.RPC("CreateLine", RpcTarget.AllBuffered, _currentPoint);
#else
            CreateLine(_currentPoint);
#endif
        }
    }

    [PunRPC]
    private void CreateLine(Vector3 startPos)
    {
        _positionCount = 2;

        _currentLineRenderer = GenerateLineRenderer(startPos);


        StartCoroutine(ConnectLine());
    }

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

    [PunRPC]
    private IEnumerator ConnectLine()
    {
        while (_isDraw)
        {
#if _Photon
            photonView.RPC("ConnectLineHelper", RpcTarget.AllBuffered, _prevPoint, _currentPoint);
#else
            ConnectLineHelper(_prevPoint, _currentPoint);
#endif
            _prevPoint = _currentPoint;
            yield return null;
        }
    }

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

    private void StopDraw() => _isDraw = false;
}
