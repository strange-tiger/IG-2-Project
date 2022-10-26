using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintbrushDraw : MonoBehaviour
{
    public enum EBrush
    {
        PENCIL,
        ERASER,
        MAX
    }

    [SerializeField] Material _lineMaterial;
    [SerializeField] Material _eraseMaterial;
    [SerializeField] GameObject _pad;
    [SerializeField] EBrush _brush;

    private const float DEFAULT_WIDTH = 0.01f;

    private LineRenderer _currentLineRenderer;
    private Vector3 _prevPos = Vector3.zero;
    private int _positionCount = 2;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _pad)
        {
            CreateLine(collision.GetContact(0).point);
            Debug.Log("1");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == _pad)
        {
            ConnectLine(collision.GetContact(0).point);
            Debug.Log("2");
        }
    }

    private void CreateLine(Vector3 startPos)
    {
        _positionCount = 2;
        GameObject line = new GameObject("Line");
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        line.transform.parent = _pad.transform;
        line.transform.position = startPos;

        float width;
        switch(_brush)
        {
            case EBrush.PENCIL:
                width = DEFAULT_WIDTH;
                lineRenderer.material = _lineMaterial;
                break;
            case EBrush.ERASER:
                width = DEFAULT_WIDTH * 5f;
                lineRenderer.material = _eraseMaterial;
                break;
            default:
                Debug.LogError("연필 종류 잘못 설정했다.");
                return;
        }

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.numCornerVertices = 5;
        lineRenderer.numCapVertices = 5;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos);

        _currentLineRenderer = lineRenderer;
    }

    private void ConnectLine(Vector3 endPos)
    {
        if (_prevPos != null && Mathf.Abs(Vector3.Distance(_prevPos, endPos)) >= 0.001f)
        {
            _prevPos = endPos;
            _positionCount++;
            _currentLineRenderer.positionCount = _positionCount;
            _currentLineRenderer.SetPosition(_positionCount - 1, endPos);
        }
    }
}
