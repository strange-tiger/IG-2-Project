using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPileForTutorial : InteracterableObject
{
    private WoodForTutorial[] _woods;
    private int _nextWood = 0;

    private static readonly YieldInstruction INTERACT_COOLTIME = new WaitForSeconds(3f);
    private static readonly Vector3[] SPAWN_DIRECTION = new Vector3[4] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    private const float SPAWN_WOOD_FORCE = 1f;
    private bool _onCooltime = false;

    private static readonly Vector3 _originalWoodPosition = new Vector3(0f, 0.5f, 0f);

    protected override void Awake()
    {
        base.Awake();
        _woods = GetComponentsInChildren<WoodForTutorial>();
    }

    public override void Interact()
    {
        if(_onCooltime)
        {
            return;
        }

        SpawnWood();
    }

    private void SpawnWood()
    {
        Vector3 spawnDirection = 2f * Vector3.up + SPAWN_DIRECTION[Random.Range(0, 4)];

        GameObject wood = _woods[_nextWood].gameObject;
        _nextWood = (_nextWood + 1) % _woods.Length;

        wood.SetActive(true);
        wood.transform.localPosition = _originalWoodPosition;
        wood.GetComponent<Rigidbody>().AddForce(SPAWN_WOOD_FORCE * spawnDirection, ForceMode.Impulse);

        StartCoroutine(CalculateCooltime());
    }

    private IEnumerator CalculateCooltime()
    {
        _onCooltime = true;

        yield return INTERACT_COOLTIME;

        _onCooltime = false;
    }

    public void ResetWood()
    {
        foreach (WoodForTutorial wood in _woods)
        {
            wood.gameObject.SetActive(false);
        }
    }
}
