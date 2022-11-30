using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIconMarkerMovement : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 5f;
    [SerializeField] private float _moveSpeed = 1.5f;
    private float _originalY;

    private void Awake()
    {
        _originalY = transform.localPosition.y;
        StartCoroutine(CoMarkerFloat());
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(CoMarkerFloat());
    }

    private IEnumerator CoMarkerFloat()
    {
        float startYPosition = 0f;
        float endYPosition = 5f;
        float currentYPosition = startYPosition;

        float elapsedTime = 0f;
        while(true)
        {
            elapsedTime += Time.deltaTime;
            currentYPosition = Mathf.Lerp(startYPosition, endYPosition, elapsedTime * _moveSpeed);
            if(Mathf.Abs(endYPosition - currentYPosition) < 0.01f)
            {
                currentYPosition = endYPosition;
                endYPosition = startYPosition;
                startYPosition = currentYPosition;

                elapsedTime = 0f;
            }
            transform.localPosition = 
                new Vector3(transform.localPosition.x,
                _originalY + currentYPosition, transform.localPosition.z);
            yield return null;
        }
    }
}
