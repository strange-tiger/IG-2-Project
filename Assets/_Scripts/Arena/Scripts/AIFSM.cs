using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFSM : MonoBehaviour
{
    private Animator _animator;
    private Collider _collider;

    private int a;
    private int z;
    private float c;
    private bool b;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();

        _animator.SetBool(AIAnimatorID.Run, true);

    }

    void Update()
    {
        c += Time.deltaTime;
        if (!b)
        {
            if (c >= 2f)
            {
                a = Random.Range(0, 361);
                transform.Rotate(new Vector3(0, a, 0));
                c -= c;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AI")
        {
            b = true;
            z = Random.Range(0, 3);

            switch (z)
            {
                case 0:
                    _animator.SetInteger(AIAnimatorID.Attack1, 1);
                    transform.LookAt(other.gameObject.transform);
                    _collider.enabled = false;
                    break;

                case 1:
                    _animator.SetInteger(AIAnimatorID.Attack2, 5);
                    transform.LookAt(other.gameObject.transform);
                    _collider.enabled = false;
                    break;

                case 2:
                    _animator.SetInteger(AIAnimatorID.Skill, 0);
                    transform.LookAt(other.gameObject.transform);
                    _collider.enabled = false;
                    break;
            }


        }
    }



}
