//#define _Photon
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PaintbrushReset : MonoBehaviourPun
{
    [SerializeField] Button _resetButton;
    [SerializeField] Material _lineMaterial;

    public event Action OnReset;

    private const float DEFAULT_WIDTH = 0.01f;

    private int _memoryChildCount;
    //private List<int> _memoryLinePointNums;
    private List<Vector3[]> _memoryLinePoints;
    private List<LineRenderer> _memoryLines;

    private void OnEnable()
    {
        _resetButton.onClick.RemoveListener(ResetPad);
        _resetButton.onClick.AddListener(ResetPad);
    }

    private void OnDisable()
    {
        _resetButton.onClick.RemoveListener(ResetPad);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.activeSelf);
            stream.SendNext(transform.childCount);

            for (int i = 1; i < transform.childCount; ++i)
            {
                LineRenderer childLineRenderer = transform.GetChild(i).GetComponent<LineRenderer>();
                int pointCount = childLineRenderer.positionCount;

                //stream.SendNext(pointCount);

                Vector3[] linePoints = new Vector3[pointCount];
                childLineRenderer.GetPositions(linePoints);

                stream.SendNext(linePoints);
            }
        }
        else if (stream.IsReading)
        {
            gameObject.SetActive((bool)stream.ReceiveNext());
            _memoryChildCount = (int)stream.ReceiveNext();

            MatchChildNum();

            for (int i = 1; i < _memoryChildCount; ++i)
            {
                //_memoryLinePointNums[i] = (int)stream.ReceiveNext();

                _memoryLinePoints[i] = (Vector3[])stream.ReceiveNext();
            }

            StopCoroutine(ReceiveLines());
            StartCoroutine(ReceiveLines());
        }
    }

    private void MatchChildNum()
    {
        for (int i = transform.childCount; i < _memoryChildCount; ++i)
        {
            _memoryLines.Add(GenerateLineRenderer());
        }

        for (int i = transform.childCount - 1; i > _memoryChildCount; --i)
        {
            Destroy(transform.GetChild(i));
        }
    }

    private LineRenderer GenerateLineRenderer()
    {
        GameObject line = new GameObject("Line");
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        line.transform.parent = transform;
        line.transform.position = Vector3.zero;

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = DEFAULT_WIDTH;
        lineRenderer.endWidth = DEFAULT_WIDTH;
        lineRenderer.numCornerVertices = 5;
        lineRenderer.numCapVertices = 5;
        lineRenderer.material = _lineMaterial;
        
        return lineRenderer;
    }

    private IEnumerator ReceiveLines()
    {
        int count = _memoryLines.Count;

        while(count >= 0)
        {
            --count;

            _memoryLines[count].SetPositions(_memoryLinePoints[count]);

            yield return null;
        }
    }

    private void ResetPad()
    {
#if _Photon
        photonView.RPC("ResetDraw", RpcTarget.All);
#else
        ResetDraw();
#endif
    }

    [PunRPC]
    private void ResetDraw()
    {
        for(int i = 1; i < transform.childCount; ++i)
        {
            PhotonNetwork.Destroy(transform.GetChild(i).gameObject);
        }
        OnReset.Invoke();
    }
}
