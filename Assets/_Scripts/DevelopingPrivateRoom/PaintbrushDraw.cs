using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PaintbrushDraw : MonoBehaviourPun
{
    [SerializeField] Material _lineMaterial;
    [SerializeField] PaintbrushReset _pad;
    [SerializeField] LayerMask _padMask;

    private const float RAY_LENGTH = 0.2f;
    private const float DEFAULT_WIDTH = 0.01f;
    private const float POINTS_DISTANCE = 0.001f;
    private static readonly Vector3 RAY_ORIGIN = new Vector3(0f, 0f, 0.1f);

    private LineRenderer _currentLineRenderer;

    private RaycastHit _hit;
    private Ray _ray;

    private Vector3 _currentPoint = Vector3.zero;
    private Vector3 _prevPoint = Vector3.zero;

    private bool _isDraw = false;
    private int _positionCount = 2;

    private void OnEnable()
    {
        _pad.OnReset -= StopDraw;
        _pad.OnReset += StopDraw;
        _ray = new Ray(transform.position + RAY_ORIGIN, transform.forward);
    }

    private void OnDisable()
    {
        _pad.OnReset -= StopDraw;
    }

    private void Update()
    {
        _ray.origin = transform.position + RAY_ORIGIN;
        Debug.DrawRay(transform.position + RAY_ORIGIN, transform.forward);
        
        if (Physics.Raycast(_ray, out _hit, RAY_LENGTH, _padMask.value))
        {
            _currentPoint = _hit.point;
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
            CreateLine(_currentPoint);
        }
    }

    [PunRPC]
    private void CreateLine(Vector3 startPos)
    {
        _positionCount = 2;
        GameObject line = new GameObject("Line");
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        line.transform.parent = _pad.transform;
        line.transform.position = startPos;

        lineRenderer.startWidth = DEFAULT_WIDTH;
        lineRenderer.endWidth = DEFAULT_WIDTH;
        lineRenderer.numCornerVertices = 5;
        lineRenderer.numCapVertices = 5;
        lineRenderer.material = _lineMaterial;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos);

        _currentLineRenderer = lineRenderer;

        StartCoroutine(ConnectLine());
    }

    [PunRPC]
    private IEnumerator ConnectLine()
    {
        while (_isDraw)
        {
            if (_prevPoint != null && Mathf.Abs(Vector3.Distance(_prevPoint, _currentPoint)) >= POINTS_DISTANCE)
            {
                _prevPoint = _currentPoint;
                _positionCount++;
                _currentLineRenderer.positionCount = _positionCount;
                _currentLineRenderer.SetPosition(_positionCount - 1, _currentPoint);
            }
            yield return null;
        }
    }

    private void StopDraw() => _isDraw = false;
}
