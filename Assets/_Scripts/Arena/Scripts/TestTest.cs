using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestTest : MonoBehaviourPun
{
    [SerializeField] private float _reStartTime;

    private bool _isStart;

    private WaitForSeconds _wait;

    private void Start()
    {
        _wait = new WaitForSeconds(_reStartTime);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !_isStart)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("¤·¤¾");
                PhotonNetwork.Instantiate("Tournament", Vector3.zero, Quaternion.identity);
                StartCoroutine(CanReStart());
                _isStart = true;
            }
        }
    }

    IEnumerator CanReStart()
    {
        yield return _wait;

        _isStart = false;
    }
}
