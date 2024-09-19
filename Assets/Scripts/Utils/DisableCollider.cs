using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollider : MonoBehaviour
{

    private Collider[] colliders;

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    public void DisableColliders()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void EnableColliders()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }



}
