using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlate : MonoBehaviour,ITrigger {

    bool active = false;
    private float Mass = 0.0f;

    [SerializeField]
    float massToTrigger = 0.1f;
    Animator animator;
    public void Awake()
    {
        animator = GetComponent<Animator>();
}
    public bool Trigger()
    {
        return (Mass >= massToTrigger);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Mass += collision.rigidbody.mass;
        if (!active && Trigger()) { animator.SetTrigger("Drop"); active = true; }
    }
    private void OnCollisionExit(Collision collision)
    {
        Mass -= collision.rigidbody.mass;
        if (active && !Trigger()) { animator.SetTrigger("Rise"); active = false; }
    }
}
