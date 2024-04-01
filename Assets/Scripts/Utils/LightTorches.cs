using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTorches : MonoBehaviour
{
    private Animator ani;

    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ani.SetBool("on", true);
    }
}
