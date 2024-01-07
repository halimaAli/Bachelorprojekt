using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed = 40.0f;

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }



}
