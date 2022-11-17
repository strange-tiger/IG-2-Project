using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SummonCircle : MonoBehaviourPun
{
    [SerializeField] IsekaiObject[] _objects;

    private static readonly WaitForSeconds SPAWN_DELAY = new WaitForSeconds(1f);
    private static readonly Vector3 FLOAT_POSITION = new Vector3(0f, 1.2f, 0f);
    private static readonly Vector3 WAIT_POSITION = new Vector3(0f, -1.5f, 0f);
    private const float RISE_TIME = 1f;

    private void OnEnable()
    {
        foreach (IsekaiObject obj in _objects)
        {
            obj.ObjectSlashed -= SpawnRPCHelper;
            obj.ObjectSlashed += SpawnRPCHelper;

            obj.gameObject.SetActive(false);
        }

        SpawnRPCHelper();
    }

    private void SpawnRPCHelper() => photonView.RPC("SpawnHelper", RpcTarget.AllBuffered);

    private void SpawnHelper() => StartCoroutine(SpawnObject());

    private int _currentIndex = 0;
    private float _elapsedTime = 0f;
    private IEnumerator SpawnObject()
    {
        yield return SPAWN_DELAY;

        _currentIndex = Random.Range(0, _objects.Length);

        _objects[_currentIndex].gameObject.SetActive(true);
        
        while (_elapsedTime <= RISE_TIME)
        {
            _elapsedTime += Time.deltaTime;
            
            _objects[_currentIndex].transform.position = Vector3.Lerp(WAIT_POSITION, FLOAT_POSITION, _elapsedTime);

            yield return null;
        }

        _elapsedTime = 0f;
    }
}
