using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            print("hit the wall");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            print("is trigger?");
        }
    }
}
