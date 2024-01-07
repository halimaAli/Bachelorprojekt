using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    [SerializeField] private float forwardLimit;
    [SerializeField] private float backLimit;

    // Update is called once per frame
    void Update()
    {
        // Destroy dogs if x position less than left limit
       /* if (transform.position.x > forwardLimit || transform.position.x < backLimit)
        {
            Destroy(gameObject);
        }*/
       
    }

    private void OnBecameInvisible()
    {
        Debug.Log("Hi");
        Destroy(gameObject);
    }
}
